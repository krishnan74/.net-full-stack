import { QuizQuestionModel } from './quizQuestion.model';
export class QuizModel {
  constructor(
    public id: number = 0,
    public title: string = '',
    public description: string = '',
    public quizQuestions?: QuizQuestionModel[],
    public teacherId: string = '',
    public createdBy: string = '',
    public createdAt: Date = new Date(),
    public dueDate: Date = new Date(),
    public isActive: boolean = true
  ) {}
}
