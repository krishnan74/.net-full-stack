import { QuizSubmission } from '../../quiz/models/quizSubmission';

export class Student {
  constructor(
    public id: string = '',
    public email: string = '',
    public firstName: string = '',
    public lastName: string = '',
    public classs: string = '',
    public createdAt: Date = new Date(),
    public quizSubmissions?: QuizSubmission[]
  ) {}
}
