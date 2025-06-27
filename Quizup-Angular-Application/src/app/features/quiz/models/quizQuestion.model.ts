import { QuestionModel } from './question.model';

export class QuizQuestionModel {
  constructor(
    public id: number = 0,
    public question: QuestionModel,
    public questionId: number = 0,
    public quizId: number = 0
  ) {}
}
