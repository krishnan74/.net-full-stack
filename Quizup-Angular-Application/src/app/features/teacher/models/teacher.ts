import { ClassModel } from '../../class/models/class';
import { QuizModel } from '../../quiz/models/quiz.model';
import { SubjectModel } from '../../subject/models/subject';

export class TeacherModel {
  constructor(
    public id: string = '',
    public email: string = '',
    public firstName: string = '',
    public lastName: string = '',
    public subjects?: SubjectModel[],
    public classes?: ClassModel[],
    public createdAt: Date = new Date(),
    public quizzes?: QuizModel[]
  ) {}
}
