import { BehaviorSubject, Observable } from 'rxjs';
import { UserModel } from './user.model';
import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';

@Injectable()
export class UserService {
  private http = inject(HttpClient);
  private usersSubject = new BehaviorSubject<UserModel[]>([]);
  users$: Observable<UserModel[]> = this.usersSubject.asObservable();

  getUsers(): void {
    this.http
      .get<any>('https://dummyjson.com/users')
      .subscribe((response: any) => {
        const users = response.users.map((user: any) =>
          UserModel.mapUserData(user)
        );
        console.log(users);
        this.usersSubject.next(users);
      });
  }

  addUser(userData: Omit<UserModel, 'id'>): void {
    this.http
      .post<any>('https://dummyjson.com/users/add', userData)
      .subscribe((response: any) => {
        const user = UserModel.mapUserData(response);
        console.log(this.users$);
        this.usersSubject.next([user, ...this.usersSubject.value]);
        console.log('User added successfully:', user);
        console.log(this.users$);
      });
  }
}
