import { ClassModel } from '../../class/models/class';
import { QuizSubmission } from '../../quiz/models/quizSubmission.model';

export class StudentModel {
  constructor(
    public id: number = 0,
    public email: string = '',
    public firstName: string = '',
    public lastName: string = '',
    public classe: ClassModel = new ClassModel(),
    public createdAt: Date = new Date(),
    public quizSubmissions?: QuizSubmission[]
  ) {}
}
