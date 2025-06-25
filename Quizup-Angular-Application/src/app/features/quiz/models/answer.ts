export class Answer {
  constructor(
    public id: string = '',
    public questionId: string = '',
    public quizSubmissionId: string = '',
    public selectedAnswer: string = ''
  ) {}
}
