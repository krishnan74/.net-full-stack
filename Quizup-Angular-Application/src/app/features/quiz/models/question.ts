export class Question {
  constructor(
    public id: string = '',
    public questionText: string = '',
    public options: string[] = [],
    public correctAnswer: string = ''
  ) {}
}
