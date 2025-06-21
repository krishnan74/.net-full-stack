
CREATE OR REPLACE FUNCTION get_system_quiz_summary(
    p_start_date TIMESTAMP DEFAULT NULL,
    p_end_date TIMESTAMP DEFAULT NULL
)
RETURNS TABLE (
    total_teachers INTEGER,
    total_students INTEGER,
    total_quizzes INTEGER,
    total_questions INTEGER,
    total_submissions INTEGER,
    total_completed_submissions INTEGER,
    total_in_progress_submissions INTEGER,
    total_saved_submissions INTEGER,
    average_completion_rate DECIMAL(5,2),
    average_student_score DECIMAL(5,2),
    system_accuracy_percentage DECIMAL(5,2),
    most_active_teachers JSON,
    most_active_students JSON,
    quiz_activity_by_month JSON,
    top_performing_quizzes JSON
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
            json_agg(
                json_build_object(
                    'teacher_id', t."Id",
                    'teacher_name', CONCAT(t."FirstName", ' ', t."LastName"),
                    'teacher_email', t."Email",
                    'quiz_count', COUNT(DISTINCT q."Id"),
                    'submission_count', COUNT(qs."Id"),
                    'avg_score', AVG(qs."Score")
                ) ORDER BY COUNT(qs."Id") DESC
                LIMIT 10
            ) as top_teachers
        FROM teachers t
        JOIN quizzes q ON t."Id" = q."TeacherId"
        LEFT JOIN quizSubmission qs ON q."Id" = qs."QuizId"
        WHERE t."IsDeleted" = false AND q."IsDeleted" = false
        GROUP BY t."Id", t."FirstName", t."LastName", t."Email"
    ),
    most_active_students_detail AS (
        SELECT 
            json_agg(
                json_build_object(
                    'student_id', s."Id",
                    'student_name', CONCAT(s."FirstName", ' ', s."LastName"),
                    'student_email', s."Email",
                    'student_class', s."Class",
                    'quiz_count', COUNT(DISTINCT qs."QuizId"),
                    'avg_score', AVG(qs."Score"),
                    'completion_rate', COUNT(DISTINCT CASE WHEN qs."SubmissionStatus" = 'Completed' THEN qs."QuizId" END)::DECIMAL / COUNT(DISTINCT qs."QuizId") * 100
                ) ORDER BY COUNT(DISTINCT qs."QuizId") DESC
                LIMIT 10
            ) as top_students
        FROM students s
        JOIN quizSubmission qs ON s."Id" = qs."StudentId"
        WHERE s."IsDeleted" = false
        GROUP BY s."Id", s."FirstName", s."LastName", s."Email", s."Class"
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
                    'completion_rate', COUNT(DISTINCT CASE WHEN qs."SubmissionStatus" = 'Completed' THEN qs."StudentId" END)::DECIMAL / COUNT(DISTINCT qs."StudentId") * 100
                ) ORDER BY COUNT(qs."Id") DESC
                LIMIT 10
            ) as top_performing
        FROM quizzes q
        JOIN teachers t ON q."TeacherId" = t."Id"
        LEFT JOIN quizSubmission qs ON q."Id" = qs."QuizId"
        WHERE q."IsDeleted" = false AND t."IsDeleted" = false
        GROUP BY q."Id", q."Title", t."FirstName", t."LastName"
    )
    SELECT 
        tc.total_teachers,
        sc.total_students,
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
        COALESCE(tq.top_performing, '[]'::json) as top_performing_quizzes
    FROM teacher_count tc
    CROSS JOIN student_count sc
    CROSS JOIN quiz_count qc
    CROSS JOIN question_count quc
    CROSS JOIN submission_stats ss
    CROSS JOIN accuracy_stats ast
    CROSS JOIN most_active_teachers_detail matd
    CROSS JOIN most_active_students_detail masd
    CROSS JOIN monthly_activity ma
    CROSS JOIN top_quizzes tq;
END;
$$ LANGUAGE plpgsql;


