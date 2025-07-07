import { QuestionModel } from './question.model';
import { QuizQuestionModel } from './quizQuestion.model';
export class QuizModel {
  constructor(
    public id: number = 0,
    public title: string = '',
    public description: string = '',
    public quizQuestions?: QuizQuestionModel[],
    public teacherId: number = 0,
    public createdBy: string = '',
    public createdAt: Date = new Date(),
    public dueDate: Date = new Date(),
    public isActive: boolean = true
  ) {}
}

export class QuizCreateModel {
  constructor(
    public title: string = '',
    public description: string = '',
    public dueDate: string,
    public teacherId: number = 0,
    public subjectId: number = 0,
    public classId: number = 0,
    public questions: QuestionModel[] = []
  ) {}
}
