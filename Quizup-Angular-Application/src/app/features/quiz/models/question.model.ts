export class QuestionModel {
  constructor(
    public id: number | null = null,
    public text: string = '',
    public options: string[] = [],
    public correctAnswer: string | null = null
  ) {}
}
