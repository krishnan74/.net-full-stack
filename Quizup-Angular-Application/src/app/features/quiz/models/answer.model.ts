export class AnswerModel {
  constructor(
    public id: number = 0,
    public questionId: string = '',
    public quizSubmissionId: string = '',
    public selectedAnswer: string = ''
  ) {}
}
