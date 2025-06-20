using QuizupAPI.Models;
using QuizupAPI.Models.DTOs.Question;

namespace QuizupAPI.Misc.Mappers
{
    public class QuestionMapper
    {
        public virtual Question? MapQuestionAddRequestQuestion( QuestionAddRequestDTO addRequestDto)
        {
            Question question = new();
            question.Text = addRequestDto.Text;
            question.Options = addRequestDto.Options;
            question.CorrectAnswer = addRequestDto.CorrectAnswer;

            return question;
        }

        public Question? MapQuestionUpdateRequestQuestion(Question existingQuestion, QuestionUpdateRequestDTO updateRequestDto)
        {
            existingQuestion.Text = updateRequestDto.Text;
            existingQuestion.Options = updateRequestDto.Options;
            existingQuestion.CorrectAnswer = updateRequestDto.CorrectAnswer;

            return existingQuestion;
        }
    }
}