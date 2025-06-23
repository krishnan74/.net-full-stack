import { Question } from './question';

export class Quiz {
  constructor(
    public id: string = '',
    public title: string = '',
    public description: string = '',
    public questions?: Question[],
    public teacherId: string = '',
    public createdBy: string = '',
    public createdAt: Date = new Date(),
    public dueDate: Date = new Date(),
    public isActive: boolean = true
  ) {}
}
