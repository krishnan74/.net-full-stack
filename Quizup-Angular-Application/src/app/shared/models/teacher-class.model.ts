import { ClassModel } from '../../features/class/models/class.model';
import { TeacherModel } from '../../features/teacher/models/teacher.model';

export class TeacherClassModel {
  constructor(
    public id: number = 0,
    public teacherId: number = 0,
    public teacher: TeacherModel = new TeacherModel(),

    public classId: number = 0,
    public classe: ClassModel = new ClassModel()
  ) {}
}
