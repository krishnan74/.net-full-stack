import { ClassModel } from '../../features/class/models/class';
import { SubjectModel } from '../../features/subject/models/subject';

export class ClassSubjectModel {
  constructor(
    public id: number = 0,
    public classId: number = 0,
    public subjectId: number = 0,
    public classe: ClassModel = new ClassModel(),
    public subject: SubjectModel = new SubjectModel()
  ) {}
}
