import { AnswerModel } from './answer.model';

export class QuizSubmission {
  constructor(
    public id: number = 0,
    public quizId: number = 0,
    public studentId: number = 0,
    public answers: AnswerModel[],
    public submissionDate: Date = new Date(),
    public savedDate: Date = new Date(),
    public score: number = 0,
    public submissionStatus: string = ''
  ) {}
}
