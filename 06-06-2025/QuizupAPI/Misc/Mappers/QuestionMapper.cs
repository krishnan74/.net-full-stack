using QuizupAPI.Models;
using QuizupAPI.Models.DTOs.Question;

namespace QuizupAPI.Misc.Mappers
{
    public class QuestionMapper
    {
        public Question? MapQuestionAddRequestQuestion( QuestionAddRequestDTO addRequestDto)
        {
            Question question = new();
            question.Text = addRequestDto.Text;
            question.Options = addRequestDto.Options;
            question.CorrectAnswer = addRequestDto.CorrectAnswer;

            return question;
        }

        public Question? MapQuestionUpdateRequestQuestion( QuestionUpdateRequestDTO updateRequestDto)
        {
            Question question = new();
            question.Text = updateRequestDto.Text;
            question.Options = updateRequestDto.Options;
            question.CorrectAnswer = updateRequestDto.CorrectAnswer;
            
            return question;
        }
    }
}