import { QuizModel } from '../../quiz/models/quiz.model';

export class Teacher {
  constructor(
    public id: string = '',
    public email: string = '',
    public firstName: string = '',
    public lastName: string = '',
    public subject: string = '',
    public createdAt: Date = new Date(),
    public quizzes?: QuizModel[]
  ) {}
}
