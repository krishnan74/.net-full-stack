import { TeacherClassModel } from '../../../shared/models/teacher-class.model';
import { TeacherSubjectModel } from '../../../shared/models/teacher-subject.model';
import { ClassModel } from '../../class/models/class.model';
import { QuizModel } from '../../quiz/models/quiz.model';

export class TeacherModel {
  constructor(
    public id: number = 0,
    public email: string = '',
    public firstName: string = '',
    public lastName: string = '',
    public teacherSubjects?: TeacherSubjectModel[],
    public teacherClasses?: TeacherClassModel[],
    public classes?: ClassModel[],
    public createdAt: Date = new Date(),
    public quizzes?: QuizModel[]
  ) {}
}

export class TeacherUpdateModel {
  constructor(
    public id: number = 0,
    public firstName: string = '',
    public lastName: string = '',
    public addSubjectIds?: number[],
    public removeSubjectIds?: number[],
    public addClassIds?: number[],
    public removeClassIds?: number[]
  ) {}
}

export class TeacherCreateModel {
  constructor(
    public email: string = '',
    public password: string = '',
    public firstName: string = '',
    public lastName: string = '',
    public subjectIds?: number[],
    public classIds?: number[]
  ) {}
}