export class QuestionModel {
  constructor(
    public id: number = 0,
    public text: string = '',
    public options: string[] = [],
    public correctAnswer: string = ''
  ) {}
}
