export class UserModel {
  constructor(
    public id: number = 0,
    public username: string = '',
    public thumbnail: string = '',
    public email: string = '',
    public password: string = '',
    public role: string = ''
  ) {}
}
