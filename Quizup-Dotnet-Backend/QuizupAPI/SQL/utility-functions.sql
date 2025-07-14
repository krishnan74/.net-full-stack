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


