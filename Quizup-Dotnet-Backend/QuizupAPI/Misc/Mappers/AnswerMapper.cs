using QuizupAPI.Models;
using QuizupAPI.Models.DTOs.Answer;

namespace QuizupAPI.Misc.Mappers
{
    public class AnswerMapper
    {
        public Answer? MapAnswerDTOToAnswer( AnswerAddRequestDTO addRequestDto, long quizSubmissionId )
        {
            Answer answer = new();
            answer.QuestionId = addRequestDto.QuestionId;
            answer.QuizSubmissionId = quizSubmissionId;
            answer.SelectedAnswer = addRequestDto.SelectedAnswer;

            return answer;
        }

    }
}