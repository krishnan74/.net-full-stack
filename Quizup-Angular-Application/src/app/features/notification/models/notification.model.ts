export class QuizNotificationModel {
  constructor(
    public id: number = 0,
    public teacherName: string = '',
    public subjectName: string = '',
    public subjectCode: string = '',
    public type: 'start' | 'end',
    public className: string = '',
    public title: string = '',
    public description: string = '',
    public createdAt: Date = new Date(),
    public dueDate?: Date | null
  ) {}
}
