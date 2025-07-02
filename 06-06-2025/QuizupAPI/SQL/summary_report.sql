CREATE OR REPLACE FUNCTION get_system_quiz_summary(
    p_start_date TIMESTAMP DEFAULT NULL,
    p_end_date TIMESTAMP DEFAULT NULL
)
RETURNS TABLE (
    "TotalTeachers" INTEGER,
    "TotalStudents" INTEGER,
    "TotalClasses" INTEGER,
    "TotalSubjects" INTEGER,
    "TotalQuizzes" INTEGER,
    "TotalQuestions" INTEGER,
    "TotalSubmissions" INTEGER,
    "TotalCompletedSubmissions" INTEGER,
    "TotalInProgressSubmissions" INTEGER,
    "TotalSavedSubmissions" INTEGER,
    "AverageCompletionRate" DECIMAL(5,2),
    "AverageStudentScore" DECIMAL(5,2),
    "SystemAccuracyPercentage" DECIMAL(5,2),
    "MostActiveTeachers" JSON,
    "MostActiveStudents" JSON,
    "QuizActivityByMonth" JSON,
    "TopPerformingQuizzes" JSON,
    "ClassSummary" JSON,
    "SubjectSummary" JSON
) AS $$
BEGIN
    RETURN QUERY
    WITH teacher_count AS (
        SELECT COUNT(*) as total_teachers
        FROM teachers
        WHERE "IsDeleted" = false
    ),
    student_count AS (
        SELECT COUNT(*) as total_students
        FROM students
        WHERE "IsDeleted" = false
    ),
    class_count AS (
        SELECT COUNT(*) as total_classes
        FROM classes
        WHERE "IsDeleted" = false
    ),
    subject_count AS (
        SELECT COUNT(*) as total_subjects
        FROM subjects
        WHERE "IsDeleted" = false
    ),
    quiz_count AS (
        SELECT COUNT(*) as total_quizzes
        FROM quizzes
        WHERE "IsDeleted" = false
    ),
    question_count AS (
        SELECT COUNT(*) as total_questions
        FROM questions
    ),
    submission_stats AS (
        SELECT 
            COUNT(*) as total_submissions,
            COUNT(CASE WHEN "SubmissionStatus" = 'Completed' THEN 1 END) as completed_submissions,
            COUNT(CASE WHEN "SubmissionStatus" = 'InProgress' THEN 1 END) as in_progress_submissions,
            COUNT(CASE WHEN "SubmissionStatus" = 'Saved' THEN 1 END) as saved_submissions,
            AVG("Score") as avg_score
        FROM quizSubmission
    ),
    accuracy_stats AS (
        SELECT 
            COUNT(a."Id") as total_answers,
            COUNT(CASE WHEN a."SelectedAnswer" = q."CorrectAnswer" THEN 1 END) as correct_answers
        FROM answers a
        JOIN questions q ON a."QuestionId" = q."Id"
    ),
    most_active_teachers_detail AS (
        SELECT 
            json_agg(top_teacher) as top_teachers
        FROM (
            SELECT 
                json_build_object(
                    'teacher_id', t."Id",
                    'teacher_name', CONCAT(t."FirstName", ' ', t."LastName"),
                    'teacher_email', t."Email",
                    'quiz_count', COUNT(DISTINCT q."Id"),
                    'submission_count', COUNT(qs."Id"),
                    'avg_score', AVG(qs."Score")
                ) as top_teacher
            FROM teachers t
            JOIN quizzes q ON t."Id" = q."TeacherId"
            LEFT JOIN quizSubmission qs ON q."Id" = qs."QuizId"
            WHERE t."IsDeleted" = false AND q."IsDeleted" = false
            GROUP BY t."Id", t."FirstName", t."LastName", t."Email"
            ORDER BY COUNT(qs."Id") DESC
            LIMIT 10
        ) sub
    ),
    most_active_students_detail AS (
        SELECT 
            json_agg(top_student) as top_students
        FROM (
            SELECT 
                json_build_object(
                    'student_id', s."Id",
                    'student_name', CONCAT(s."FirstName", ' ', s."LastName"),
                    'student_email', s."Email",
                    'student_class', c."Name",
                    'quiz_count', COUNT(DISTINCT qs."QuizId"),
                    'avg_score', AVG(qs."Score"),
                    'completion_rate', COUNT(DISTINCT CASE WHEN qs."SubmissionStatus" = 'Completed' THEN qs."QuizId" END)::DECIMAL / NULLIF(COUNT(DISTINCT qs."QuizId"),0) * 100
                ) as top_student
            FROM students s
            LEFT JOIN classes c ON s."ClassId" = c."Id"
            JOIN quizSubmission qs ON s."Id" = qs."StudentId"
            WHERE s."IsDeleted" = false
            GROUP BY s."Id", s."FirstName", s."LastName", s."Email", c."Name"
            ORDER BY COUNT(DISTINCT qs."QuizId") DESC
            LIMIT 10
        ) sub
    ),
    monthly_activity AS (
        SELECT 
            json_agg(
                json_build_object(
                    'month', DATE_TRUNC('month', qs."SubmissionDate"),
                    'submission_count', COUNT(qs."Id"),
                    'quiz_count', COUNT(DISTINCT qs."QuizId"),
                    'student_count', COUNT(DISTINCT qs."StudentId"),
                    'avg_score', AVG(qs."Score")
                ) ORDER BY DATE_TRUNC('month', qs."SubmissionDate")
            ) as monthly_trend
        FROM quizSubmission qs
        WHERE qs."SubmissionDate" >= CURRENT_DATE - INTERVAL '12 months'
        GROUP BY DATE_TRUNC('month', qs."SubmissionDate")
    ),
    top_quizzes AS (
        SELECT 
            json_agg(
                json_build_object(
                    'quiz_id', q."Id",
                    'quiz_title', q."Title",
                    'teacher_name', CONCAT(t."FirstName", ' ', t."LastName"),
                    'submission_count', COUNT(qs."Id"),
                    'avg_score', AVG(qs."Score"),
                    'completion_rate', COUNT(DISTINCT CASE WHEN qs."SubmissionStatus" = 'Completed' THEN qs."StudentId" END)::DECIMAL / NULLIF(COUNT(DISTINCT qs."StudentId"),0) * 100
                ) ORDER BY COUNT(qs."Id") DESC
                LIMIT 10
            ) as top_performing
        FROM quizzes q
        JOIN teachers t ON q."TeacherId" = t."Id"
        LEFT JOIN quizSubmission qs ON q."Id" = qs."QuizId"
        WHERE q."IsDeleted" = false AND t."IsDeleted" = false
        GROUP BY q."Id", q."Title", t."FirstName", t."LastName"
    ),
    class_summary_detail AS (
        SELECT json_agg(
            json_build_object(
                'class_id', c."Id",
                'class_name', c."Name",
                'student_count', COUNT(s."Id"),
                'quiz_count', COUNT(DISTINCT q."Id"),
                'avg_score', AVG(qs."Score")
            ) ORDER BY c."Name"
        ) as class_summary
        FROM classes c
        LEFT JOIN students s ON c."Id" = s."ClassId" AND s."IsDeleted" = false
        LEFT JOIN quizSubmission qs ON s."Id" = qs."StudentId"
        LEFT JOIN quizzes q ON qs."QuizId" = q."Id" AND q."IsDeleted" = false
        WHERE c."IsDeleted" = false
        GROUP BY c."Id", c."Name"
    ),
    subject_summary_detail AS (
        SELECT json_agg(
            json_build_object(
                'subject_id', s."Id",
                'subject_name', s."Name",
                'teacher_count', COUNT(DISTINCT ts."TeacherId"),
                'quiz_count', COUNT(DISTINCT q."Id"),
                'avg_score', AVG(qs."Score")
            ) ORDER BY s."Name"
        ) as subject_summary
        FROM subjects s
        LEFT JOIN teacherSubjects ts ON s."Id" = ts."SubjectId"
        LEFT JOIN quizzes q ON ts."TeacherId" = q."TeacherId" AND q."IsDeleted" = false
        LEFT JOIN quizSubmission qs ON q."Id" = qs."QuizId"
        WHERE s."IsDeleted" = false
        GROUP BY s."Id", s."Name"
    )
    SELECT 
        tc.total_teachers,
        sc.total_students,
        cc.total_classes,
        suc.total_subjects,
        qc.total_quizzes,
        quc.total_questions,
        ss.total_submissions,
        ss.completed_submissions,
        ss.in_progress_submissions,
        ss.saved_submissions,
        CASE 
            WHEN ss.total_submissions > 0 
            THEN ROUND((ss.completed_submissions::DECIMAL / ss.total_submissions) * 100, 2)
            ELSE 0 
        END as average_completion_rate,
        ROUND(COALESCE(ss.avg_score, 0), 2) as average_student_score,
        CASE 
            WHEN ast.total_answers > 0 
            THEN ROUND((ast.correct_answers::DECIMAL / ast.total_answers) * 100, 2)
            ELSE 0 
        END as system_accuracy_percentage,
        COALESCE(matd.top_teachers, '[]'::json) as most_active_teachers,
        COALESCE(masd.top_students, '[]'::json) as most_active_students,
        COALESCE(ma.monthly_trend, '[]'::json) as quiz_activity_by_month,
        COALESCE(tq.top_performing, '[]'::json) as top_performing_quizzes,
        COALESCE(csd.class_summary, '[]'::json) as class_summary,
        COALESCE(ssd.subject_summary, '[]'::json) as subject_summary
    FROM teacher_count tc
    CROSS JOIN student_count sc
    CROSS JOIN class_count cc
    CROSS JOIN subject_count suc
    CROSS JOIN quiz_count qc
    CROSS JOIN question_count quc
    CROSS JOIN submission_stats ss
    CROSS JOIN accuracy_stats ast
    CROSS JOIN most_active_teachers_detail matd
    CROSS JOIN most_active_students_detail masd
    CROSS JOIN monthly_activity ma
    CROSS JOIN top_quizzes tq
    CROSS JOIN class_summary_detail csd
    CROSS JOIN subject_summary_detail ssd;
END;
$$ LANGUAGE plpgsql;


