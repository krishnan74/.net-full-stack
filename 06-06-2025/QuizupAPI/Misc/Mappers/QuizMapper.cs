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

        public Quiz? MapQuizUpdateRequestQuiz(Quiz existingQuiz, QuizUpdateRequestDTO updateRequestDto)
        {
            existingQuiz.Title = updateRequestDto.Title;
            existingQuiz.Description = updateRequestDto.Description;
            existingQuiz.DueDate = updateRequestDto.DueDate;

            return existingQuiz;
        }
         
    }
}