export class AnswerModel {
  constructor(
    public id: number = 0,
    public questionId: number = 0,
    public quizSubmissionId: string = '',
    public selectedAnswer: string = ''
  ) {}
}
