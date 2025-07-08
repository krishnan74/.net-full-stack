import { ClassSubjectModel } from '../../../shared/models/class-subject.model';
import { SubjectModel } from '../../subject/models/subject.model';

export class ClassModel {
  constructor(
    public id: number = 0,
    public name: string = '',
    public createdAt: Date = new Date(),
    public updatedAt: Date = new Date(),
    public subjectIds: number[] = [],
    public classSubjects?: ClassSubjectModel[]
  ) {}
}
