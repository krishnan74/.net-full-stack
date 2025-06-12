using QuizupAPI.Models;
using QuizupAPI.Models.DTOs.Answer;

namespace QuizupAPI.Misc.Mappers
{
    public class AnswerMapper
    {
        public Answer? MapAnswerDTOToAnswer( AnswerAddRequestDTO addRequestDto )
        {
            Answer answer = new();
            answer.QuestionId = addRequestDto.QuestionId;
            answer.QuizSubmissionId = addRequestDto.QuizSubmissionId;
            answer.SelectedAnswer = addRequestDto.SelectedAnswer;

            return answer;
        }

    }
}