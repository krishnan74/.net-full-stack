
export class SubjectModel {
  constructor(
    public id: number = 0,
    public code: string = '',
    public name: string = '',
    public createdAt: Date = new Date(),
    public updatedAt: Date = new Date(),
  ) {}
}
