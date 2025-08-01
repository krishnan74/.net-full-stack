import { ClassModel } from '../../class/models/class.model';
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

export class StudentUpdateModel {
  constructor(
    public id: number = 0,
    public firstName: string = '',
    public lastName: string = '',
    public classId: number = 0
  ) {}
}

export class StudentCreateModel {
  constructor(
    public email: string = '',
    public password: string = '',
    public firstName: string = '',
    public lastName: string = '',
    public classId: number = 0,
  ) {}
}
