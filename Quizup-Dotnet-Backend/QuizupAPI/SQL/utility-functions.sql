CREATE OR REPLACE FUNCTION get_questions_by_teacher_id(
    p_teacher_id BIGINT
)
RETURNS TABLE(
	"Id" BIGINT,
	"Text" TEXT,
	"CorrectAnswer" TEXT,
	"Options" TEXT[]
)
AS $$
BEGIN 
	RETURN QUERY
	
	SELECT qs.* FROM "quizQuestions" qq 
		JOIN quizzes qz ON qq."QuizId" = qz."Id" 
		JOIN questions qs ON qq."QuestionId" = qs."Id"
		
		WHERE qz."TeacherId" = p_teacher_id;
END;
$$ LANGUAGE plpgsql;


SELECT * FROM get_questions_by_teacher_id(1);

CREATE OR REPLACE FUNCTION get_subjects_by_student_id(p_student_id BIGINT)
RETURNS TABLE(
	"Id" BIGINT,
	"Name" TEXT,
	"Code" TEXT, 
	"CreatedAt" TIMESTAMP WITH TIME ZONE,
	"UpdatedAt" TIMESTAMP WITH TIME ZONE,
	"IsDeleted" BOOLEAN
)
AS $$

BEGIN
    RETURN QUERY
    SELECT 
        s.*
    FROM students st
    JOIN classes cl ON st."ClassId" = cl."Id"
    JOIN "classSubjects" cs ON cs."ClassId" = cl."Id"
    JOIN subjects s ON cs."SubjectId" = s."Id"
    WHERE st."Id" = p_student_id;
END;
$$ LANGUAGE plpgsql;

SELECT * FROM get_subjects_by_student_id(1);
