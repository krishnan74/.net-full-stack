export class ClassModel {
  constructor(
    public id: number = 0,
    public name: string = '',
    public createdAt: Date = new Date(),
    public updatedAt: Date = new Date(),
    public subjectIds: number[] = []
  ) {}
}
