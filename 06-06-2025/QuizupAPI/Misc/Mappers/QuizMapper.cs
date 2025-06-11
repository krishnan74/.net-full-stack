using QuizupAPI.Models;
using QuizupAPI.Models.DTOs.Quiz;

namespace QuizupAPI.Misc.Mappers
{
    public class QuizMapper
    {
        public Quiz? MapQuizAddRequestQuiz(QuizAddRequestDTO addRequestDto)
        {
            Quiz quiz = new();
            quiz.Title = addRequestDto.Title;
            quiz.Description = addRequestDto.Description;
            quiz.DueDate = addRequestDto.DueDate;
            quiz.TeacherId = addRequestDto.TeacherId;

            return quiz;
        }

        public Quiz? MapQuizUpdateRequestQuiz(QuizUpdateRequestDTO updateRequestDto)
        {
            Quiz quiz = new();
            quiz.Title = updateRequestDto.Title;
            quiz.Description = updateRequestDto.Description;            
            quiz.DueDate = updateRequestDto.DueDate;
        
            return quiz;
        }
    }
}