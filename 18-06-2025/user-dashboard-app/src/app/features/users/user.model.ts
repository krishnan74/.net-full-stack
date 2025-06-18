export class UserModel {
  constructor(
    public id: number = 0,
    public username: string = '',
    public firstName: string = '',
    public lastName: string = '',
    public email: string = '',
    public gender: string = '',
    public role: string = '',
    public state: string = ''
  ) {}

  static mapUserData(data: any): UserModel {
    return new UserModel(
      data.id,
      data.username,
      data.firstName,
      data.lastName,
      data.email,
      data.gender,
      data.role,
      data.address.state
    );
  }
}
