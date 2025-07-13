import { QuizSubmissionModel } from '../../../shared/models/quiz-submission.model';
import { ClassModel } from '../../class/models/class.model';
import { SubjectModel } from '../../subject/models/subject.model';
import { TeacherModel } from '../../teacher/models/teacher.model';
import { QuestionModel } from './question.model';
import { QuizQuestionModel } from './quizQuestion.model';
export class QuizModel {
  constructor(
    public id: number = 0,
    public title: string = '',
    public description: string = '',
    public quizQuestions?: QuizQuestionModel[],
    public submissions?: QuizSubmissionModel[],
    public subject?: SubjectModel,
    public classe?: ClassModel,
    public tags?: string[],
    public teacherId: number = 0,
    public teacher: TeacherModel = new TeacherModel(),
    public createdAt: Date = new Date(),
    public dueDate: Date = new Date(),
    public isActive: boolean = true
  ) {}
}

export class QuizCreateModel {
  constructor(
    public title: string = '',
    public description: string = '',
    public dueDate: string,
    public teacherId: number = 0,
    public subjectId: number = 0,
    public classId: number = 0,
    public questions: QuestionModel[] = []
  ) {}
}
