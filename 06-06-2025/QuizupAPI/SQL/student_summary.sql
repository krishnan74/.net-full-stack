CREATE OR REPLACE FUNCTION get_student_quiz_summary(
    p_student_id BIGINT,
    p_start_date TIMESTAMP DEFAULT NULL,
    p_end_date TIMESTAMP DEFAULT NULL
)
RETURNS TABLE (
    "StudentId" BIGINT,
    "StudentName" TEXT,
    "StudentEmail" TEXT,
    "StudentClass" VARCHAR(100),
    "TotalQuizzesAvailable" BIGINT,
    "TotalQuizzesStarted" BIGINT,
    "TotalQuizzesCompleted" BIGINT,
    "TotalQuizzesInProgress" BIGINT,
    "TotalQuizzesSaved" BIGINT,
    "AverageScore" DECIMAL(5,2),
    "HighestScore" INTEGER,
    "LowestScore" INTEGER,
    "TotalQuestionsAttempted" BIGINT,
    "TotalCorrectAnswers" BIGINT,
    "AccuracyPercentage" DECIMAL(5,2),
    "TotalTimeSpentMinutes" INTEGER,
    "QuizzesByStatus" JSON,
    "RecentActivity" JSON,
    "PerformanceTrend" JSON
) AS $$
BEGIN
    RETURN QUERY
    WITH student_info AS (
        SELECT 
            s."Id",
            CONCAT(s."FirstName", ' ', s."LastName") as full_name,
            s."Email",
            s."ClassId"
        FROM students s
        WHERE s."Id" = p_student_id AND s."IsDeleted" = false
    ),
    class_info AS (
        SELECT c."Id", c."Name" FROM classes c
    ),
    quiz_stats AS (
        SELECT 
            qs."StudentId",
            COUNT(DISTINCT qs."QuizId") as total_started,
            COUNT(DISTINCT CASE WHEN qs."SubmissionStatus" = 'Submitted' THEN qs."QuizId" END) as total_completed,
            COUNT(DISTINCT CASE WHEN qs."SubmissionStatus" = 'InProgress' THEN qs."QuizId" END) as total_in_progress,
            COUNT(DISTINCT CASE WHEN qs."SubmissionStatus" = 'Saved' THEN qs."QuizId" END) as total_saved,
            AVG(qs."Score") as avg_score,
            MAX(qs."Score") as max_score,
            MIN(qs."Score") as min_score
        FROM "quizSubmissions" qs
        WHERE qs."StudentId" = p_student_id
        GROUP BY qs."StudentId"
    ),
    question_stats AS (
        SELECT 
            qs."StudentId",
            COUNT(a."Id") as total_questions_attempted,
            COUNT(CASE WHEN a."SelectedAnswer" = q."CorrectAnswer" THEN 1 END) as total_correct_answers
        FROM "quizSubmissions" qs
        JOIN answers a ON qs."Id" = a."QuizSubmissionId"
        JOIN questions q ON a."QuestionId" = q."Id"
        WHERE qs."StudentId" = p_student_id
        GROUP BY qs."StudentId"
    ),
    available_quizzes AS (
        SELECT COUNT(DISTINCT q."Id") as total_available
        FROM quizzes q
        WHERE q."IsDeleted" = false AND q."IsActive" = true
    ),
    quizzes_by_status_detail AS (
        SELECT 
            t."StudentId",
            json_object_agg(t."SubmissionStatus", t.quiz_count) as status_breakdown
        FROM (
            SELECT 
                qs."StudentId",
                qs."SubmissionStatus",
                COUNT(DISTINCT qs."QuizId") as quiz_count
            FROM "quizSubmissions" qs
            WHERE qs."StudentId" = p_student_id
            GROUP BY qs."StudentId", qs."SubmissionStatus"
        ) t
        GROUP BY t."StudentId"
    ),
    recent_activity_detail AS (
        SELECT 
            qs."StudentId",
            json_agg(
                json_build_object(
                    'quiz_id', qs."QuizId",
                    'quiz_title', q."Title",
                    'submission_date', qs."SubmissionDate",
                    'status', qs."SubmissionStatus",
                    'score', qs."Score"
                ) ORDER BY qs."SubmissionDate" DESC
            ) FILTER (WHERE qs."SubmissionDate" >= CURRENT_DATE - INTERVAL '30 days') as recent_activities
        FROM "quizSubmissions" qs
        JOIN quizzes q ON qs."QuizId" = q."Id"
        WHERE qs."StudentId" = p_student_id
        GROUP BY qs."StudentId"
    ),
    performance_trend_detail AS (
        SELECT 
            t."StudentId",
            json_agg(
                json_build_object(
                    'month', t.month,
                    'avg_score', t.avg_score,
                    'quiz_count', t.quiz_count
                ) ORDER BY t.month
            ) as monthly_trend
        FROM (
            SELECT 
                qs."StudentId",
                DATE_TRUNC('month', qs."SubmissionDate") as month,
                AVG(qs."Score") as avg_score,
                COUNT(DISTINCT qs."QuizId") as quiz_count
            FROM "quizSubmissions" qs
            WHERE qs."StudentId" = p_student_id 
            AND qs."SubmissionStatus" = 'Submitted'
            AND qs."SubmissionDate" >= CURRENT_DATE - INTERVAL '6 months'
            GROUP BY qs."StudentId", DATE_TRUNC('month', qs."SubmissionDate")
        ) t
        GROUP BY t."StudentId"
    )
    SELECT 
        si."Id" as "StudentId",
        si.full_name as "StudentName",
        si."Email" as "StudentEmail",
        COALESCE(ci."Name", '')::VARCHAR(100) as "StudentClass",
        COALESCE(aq.total_available, 0) as "TotalQuizzesAvailable",
        COALESCE(qs.total_started, 0) as "TotalQuizzesStarted",
        COALESCE(qs.total_completed, 0) as "TotalQuizzesCompleted",
        COALESCE(qs.total_in_progress, 0) as "TotalQuizzesInProgress",
        COALESCE(qs.total_saved, 0) as "TotalQuizzesSaved",
        ROUND(COALESCE(qs.avg_score, 0), 2) as "AverageScore",
        COALESCE(qs.max_score, 0) as "HighestScore",
        COALESCE(qs.min_score, 0) as "LowestScore",
        COALESCE(qst.total_questions_attempted, 0) as "TotalQuestionsAttempted",
        COALESCE(qst.total_correct_answers, 0) as "TotalCorrectAnswers",
        CASE 
            WHEN qst.total_questions_attempted > 0 
            THEN ROUND((qst.total_correct_answers::DECIMAL / qst.total_questions_attempted) * 100, 2)
            ELSE 0 
        END as "AccuracyPercentage",
        0 as "TotalTimeSpentMinutes",
        COALESCE(qbsd.status_breakdown, '{}'::json) as "QuizzesByStatus",
        COALESCE(rad.recent_activities, '[]'::json) as "RecentActivity",
        COALESCE(ptd.monthly_trend, '[]'::json) as "PerformanceTrend"
    FROM student_info si
    LEFT JOIN class_info ci ON si."ClassId" = ci."Id"
    LEFT JOIN quiz_stats qs ON si."Id" = qs."StudentId"
    LEFT JOIN question_stats qst ON si."Id" = qst."StudentId"
    LEFT JOIN available_quizzes aq ON true
    LEFT JOIN quizzes_by_status_detail qbsd ON si."Id" = qbsd."StudentId"
    LEFT JOIN recent_activity_detail rad ON si."Id" = rad."StudentId"
    LEFT JOIN performance_trend_detail ptd ON si."Id" = ptd."StudentId";
END;
$$ LANGUAGE plpgsql;

SELECT * FROM get_student_quiz_summary(2);

