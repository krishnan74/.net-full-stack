import { AnswerModel } from './answer.model';

export class QuizSubmission {
  constructor(
    public id: number = 0,
    public quizId: string = '',
    public studentId: string = '',
    public answers: AnswerModel[],
    public submissionDate: Date = new Date(),
    public savedDate: Date = new Date(),
    public score: number = 0,
    public submissionStatus: SubmissionStatus = SubmissionStatus.IN_PROGRESS
  ) {}
}

enum SubmissionStatus {
  IN_PROGRESS = 'InProgress',
  SAVED = 'saved',
  SUBMITTED = 'submitted',
}
