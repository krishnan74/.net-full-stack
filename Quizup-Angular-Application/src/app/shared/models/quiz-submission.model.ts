import { AnswerModel } from '../../features/quiz/models/answer.model';
import { QuizModel } from '../../features/quiz/models/quiz.model';
import { StudentModel } from '../../features/student/models/student';

export class QuizSubmissionModel {
  constructor(
    public id: number = 0,
    public quizId: number = 0,
    public quiz: QuizModel,
    public student: StudentModel,
    public submissionDate: Date = new Date(),
    public savedDate: Date = new Date(),
    public answers: AnswerModel[] = [],
    public submissionStatus: string = '',
    public score: number = 0
  ) {}
}
