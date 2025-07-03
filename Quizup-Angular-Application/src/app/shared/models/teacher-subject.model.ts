import { SubjectModel } from '../../features/subject/models/subject';
import { TeacherModel } from '../../features/teacher/models/teacher';

export class TeacherSubjectModel {
  constructor(
    public id: number = 0,
    public teacherId: number = 0,
    public teacher: TeacherModel = new TeacherModel(),

    public subjectId: number = 0,
    public subject: SubjectModel = new SubjectModel()
  ) {}
}
