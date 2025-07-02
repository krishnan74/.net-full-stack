CREATE OR REPLACE FUNCTION get_teacher_quiz_summary(
    p_teacher_id BIGINT,
    p_start_date TIMESTAMP DEFAULT NULL,
    p_end_date TIMESTAMP DEFAULT NULL
)
RETURNS TABLE (
    "TeacherId" BIGINT,
    "TeacherName" TEXT,
    "TeacherEmail" TEXT,
    "TeacherSubjects" JSON,
    "TotalQuizzesCreated" BIGINT,
    "TotalActiveQuizzes" BIGINT,
    "TotalInactiveQuizzes" BIGINT,
    "TotalQuestionsCreated" BIGINT,
    "TotalStudentSubmissions" BIGINT,
    "TotalStudentsParticipated" BIGINT,
    "AverageCompletionRate" DECIMAL(5,2),
    "AverageStudentScore" DECIMAL(5,2),
    "HighestQuizScore" INTEGER,
    "LowestQuizScore" INTEGER,
    "TotalQuestionsAnswered" BIGINT,
    "TotalCorrectAnswers" BIGINT,
    "OverallAccuracyPercentage" DECIMAL(5,2),
    "QuizzesByStatus" JSON,
    "StudentPerformanceSummary" JSON,
    "RecentQuizActivity" JSON,
    "QuizPerformanceTrend" JSON
) AS $$
BEGIN
    RETURN QUERY
    WITH teacher_info AS (
        SELECT 
            t."Id",
            CONCAT(t."FirstName", ' ', t."LastName") as full_name,
            t."Email"
        FROM teachers t
        WHERE t."Id" = p_teacher_id 
    ),
    teacher_subjects_json AS (
        SELECT ts."TeacherId", json_agg(s."Name") as subjects
        FROM "teacherSubjects" ts
        JOIN subjects s ON ts."SubjectId" = s."Id"
        WHERE ts."TeacherId" = p_teacher_id
        GROUP BY ts."TeacherId"
    ),
    quiz_stats AS (
        SELECT 
            q."TeacherId",
            COUNT(DISTINCT q."Id") as total_created,
            COUNT(DISTINCT CASE WHEN q."IsActive" = true THEN q."Id" END) as total_active,
            COUNT(DISTINCT CASE WHEN q."IsActive" = false THEN q."Id" END) as total_inactive,
            COUNT(DISTINCT qq."QuestionId") as total_questions_created
        FROM quizzes q
        LEFT JOIN "quizQuestions" qq ON q."Id" = qq."QuizId"
        WHERE q."TeacherId" = p_teacher_id
        GROUP BY q."TeacherId"
    ),
    submission_stats AS (
        SELECT 
            q."TeacherId",
            COUNT(DISTINCT qs."Id") as total_submissions,
            COUNT(DISTINCT qs."StudentId") as total_students,
            COUNT(DISTINCT CASE WHEN qs."SubmissionStatus" = 'Submitted' THEN qs."QuizId" END) as completed_quizzes,
            COUNT(DISTINCT qs."QuizId") as total_quizzes_with_submissions,
            AVG(qs."Score") as avg_student_score,
            MAX(qs."Score") as max_score,
            MIN(qs."Score") as min_score
        FROM quizzes q
        JOIN "quizSubmissions" qs ON q."Id" = qs."QuizId"
        WHERE q."TeacherId" = p_teacher_id 
        GROUP BY q."TeacherId"
    ),
    question_performance AS (
        SELECT 
            q."TeacherId",
            COUNT(a."Id") as total_questions_answered,
            COUNT(CASE WHEN a."SelectedAnswer" = qu."CorrectAnswer" THEN 1 END) as total_correct_answers
        FROM quizzes q
        JOIN "quizQuestions" qq ON q."Id" = qq."QuizId"
        JOIN questions qu ON qq."QuestionId" = qu."Id"
        JOIN "quizSubmissions" qs ON q."Id" = qs."QuizId"
        JOIN answers a ON qs."Id" = a."QuizSubmissionId" AND qu."Id" = a."QuestionId"
        WHERE q."TeacherId" = p_teacher_id 
        GROUP BY q."TeacherId"
    ),
    quizzes_by_status_detail AS (
        SELECT 
            t."TeacherId",
            json_object_agg(t.status, t.quiz_count) as status_breakdown
        FROM (
            SELECT 
                q."TeacherId",
                CASE 
                    WHEN q."IsActive" = true THEN 'Active'
                    ELSE 'Inactive'
                END as status,
                COUNT(q."Id") as quiz_count
            FROM quizzes q
            WHERE q."TeacherId" = p_teacher_id
            GROUP BY q."TeacherId", status
        ) t
        GROUP BY t."TeacherId"
    ),
    student_performance_detail AS (
        SELECT 
            t."TeacherId",
            json_agg(
                json_build_object(
                    'student_id', t."StudentId",
                    'student_name', t.student_name,
                    'student_email', t."Email",
                    'student_class', c."Name",
                    'total_quizzes_taken', t.total_quizzes_taken,
                    'average_score', t.average_score,
                    'completion_rate', t.completion_rate
                ) ORDER BY t.average_score DESC
            ) as student_summary
        FROM (
            SELECT 
                q."TeacherId",
                s."Id" AS "StudentId",
                CONCAT(s."FirstName", ' ', s."LastName") AS student_name,
                s."Email",
                s."ClassId",
                COUNT(DISTINCT qs."QuizId") AS total_quizzes_taken,
                AVG(qs."Score") AS average_score,
                CASE 
                    WHEN COUNT(DISTINCT qs."QuizId") > 0 
                    THEN COUNT(DISTINCT CASE WHEN qs."SubmissionStatus" = 'Submitted' THEN qs."QuizId" END)::DECIMAL / COUNT(DISTINCT qs."QuizId") * 100
                    ELSE 0
                END AS completion_rate
            FROM quizzes q
            JOIN "quizSubmissions" qs ON q."Id" = qs."QuizId"
            JOIN students s ON qs."StudentId" = s."Id"
            WHERE q."TeacherId" = p_teacher_id 
            GROUP BY q."TeacherId", s."Id", s."FirstName", s."LastName", s."Email", s."ClassId"
        ) t
        LEFT JOIN classes c ON t."ClassId" = c."Id"
        GROUP BY t."TeacherId"
    ),
    recent_quiz_activity_detail AS (
        SELECT 
            t."TeacherId",
            json_agg(
                json_build_object(
                    'quiz_id', t."Id",
                    'quiz_title', t."Title",
                    'submission_count', t.submission_count,
                    'last_submission_date', t.last_submission_date,
                    'average_score', t.average_score
                ) ORDER BY t.last_submission_date DESC
            ) as recent_activities
        FROM (
            SELECT 
                q."TeacherId",
                q."Id",
                q."Title",
                COUNT(qs."Id") as submission_count,
                MAX(qs."SubmissionDate") as last_submission_date,
                AVG(qs."Score") as average_score
            FROM quizzes q
            LEFT JOIN "quizSubmissions" qs ON q."Id" = qs."QuizId"
            WHERE q."TeacherId" = p_teacher_id 
              AND (qs."SubmissionDate" IS NULL OR qs."SubmissionDate" >= CURRENT_DATE - INTERVAL '30 days')
            GROUP BY q."TeacherId", q."Id", q."Title"
        ) t
        GROUP BY t."TeacherId"
    ),
    quiz_performance_trend_detail AS (
        SELECT 
            t."TeacherId",
            json_agg(
                json_build_object(
                    'month', t.month,
                    'quiz_count', t.quiz_count,
                    'submission_count', t.submission_count,
                    'avg_score', t.avg_score,
                    'completion_rate', t.completion_rate
                ) ORDER BY t.month
            ) as monthly_trend
        FROM (
            SELECT 
                q."TeacherId",
                DATE_TRUNC('month', qs."SubmissionDate") AS month,
                COUNT(DISTINCT qs."QuizId") AS quiz_count,
                COUNT(qs."Id") AS submission_count,
                AVG(qs."Score") AS avg_score,
                CASE 
                    WHEN COUNT(DISTINCT qs."QuizId") > 0 
                    THEN COUNT(DISTINCT CASE WHEN qs."SubmissionStatus" = 'Submitted' THEN qs."QuizId" END)::DECIMAL / COUNT(DISTINCT qs."QuizId") * 100
                    ELSE 0
                END AS completion_rate
            FROM quizzes q
            JOIN "quizSubmissions" qs ON q."Id" = qs."QuizId"
            WHERE q."TeacherId" = p_teacher_id
              AND qs."SubmissionDate" >= CURRENT_DATE - INTERVAL '6 months'
            GROUP BY q."TeacherId", DATE_TRUNC('month', qs."SubmissionDate")
        ) t
        GROUP BY t."TeacherId"
    )
    SELECT 
        ti."Id" as "TeacherId",
        ti.full_name as "TeacherName",
        ti."Email" as "TeacherEmail",
        COALESCE(tsj.subjects, '[]'::json) as "TeacherSubjects",
        COALESCE(qs.total_created, 0) as "TotalQuizzesCreated",
        COALESCE(qs.total_active, 0) as "TotalActiveQuizzes",
        COALESCE(qs.total_inactive, 0) as "TotalInactiveQuizzes",
        COALESCE(qs.total_questions_created, 0) as "TotalQuestionsCreated",
        COALESCE(ss.total_submissions, 0) as "TotalStudentSubmissions",
        COALESCE(ss.total_students, 0) as "TotalStudentsParticipated",
        CASE 
            WHEN ss.total_quizzes_with_submissions > 0 
            THEN ROUND((ss.completed_quizzes::DECIMAL / ss.total_quizzes_with_submissions) * 100, 2)
            ELSE 0 
        END as "AverageCompletionRate",
        ROUND(COALESCE(ss.avg_student_score, 0), 2) as "AverageStudentScore",
        COALESCE(ss.max_score, 0) as "HighestQuizScore",
        COALESCE(ss.min_score, 0) as "LowestQuizScore",
        COALESCE(qp.total_questions_answered, 0) as "TotalQuestionsAnswered",
        COALESCE(qp.total_correct_answers, 0) as "TotalCorrectAnswers",
        CASE 
            WHEN qp.total_questions_answered > 0 
            THEN ROUND((qp.total_correct_answers::DECIMAL / qp.total_questions_answered) * 100, 2)
            ELSE 0 
        END as "OverallAccuracyPercentage",
        COALESCE(qbsd.status_breakdown, '{}'::json) as "QuizzesByStatus",
        COALESCE(spd.student_summary, '[]'::json) as "StudentPerformanceSummary",
        COALESCE(raqd.recent_activities, '[]'::json) as "RecentQuizActivity",
        COALESCE(qptd.monthly_trend, '[]'::json) as "QuizPerformanceTrend"
    FROM teacher_info ti
    LEFT JOIN teacher_subjects_json tsj ON ti."Id" = tsj."TeacherId"
    LEFT JOIN quiz_stats qs ON ti."Id" = qs."TeacherId"
    LEFT JOIN submission_stats ss ON ti."Id" = ss."TeacherId"
    LEFT JOIN question_performance qp ON ti."Id" = qp."TeacherId"
    LEFT JOIN quizzes_by_status_detail qbsd ON ti."Id" = qbsd."TeacherId"
    LEFT JOIN student_performance_detail spd ON ti."Id" = spd."TeacherId"
    LEFT JOIN recent_quiz_activity_detail raqd ON ti."Id" = raqd."TeacherId"
    LEFT JOIN quiz_performance_trend_detail qptd ON ti."Id" = qptd."TeacherId";
END;
$$ LANGUAGE plpgsql;

SELECT * FROM get_teacher_quiz_summary(2);

