import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { UserModel } from './user.model';
import { inject, Injectable } from '@angular/core';

@Injectable()
export class UserService {
  private usersSubject = new BehaviorSubject<UserModel[]>([
    new UserModel(
      1,
      'markgrayson',
      'https://placehold.co/150',
      'markgrayson@gmail.com',
      'password123',
      'admin'
    ),
    new UserModel(
      2,
      'sarahconnor',
      'https://placehold.co/150',
      'sarahconnor@gmail.com',
      'password123',
      'user'
    ),
  ]);

  users$: Observable<UserModel[]> = this.usersSubject.asObservable();

  getUserSearchResult(
    searchData: string,
    limit: number = 10,
    skip: number = 0
  ): Observable<any> {
    return this.users$.pipe(
      map((users) => {
        const filteredUsers = users.filter((user) =>
          user.username.toLowerCase().includes(searchData.toLowerCase())
        );
        const paginatedUsers = filteredUsers.slice(skip, skip + limit);
        return {
          users: paginatedUsers,
          total: filteredUsers.length,
        };
      })
    );
  }

  addUser(userData: Omit<UserModel, 'id'>): void {
    const newUser = new UserModel(
      this.usersSubject.value.length + 1,
      userData.username,
      userData.email,
      userData.password,
      userData.role
    );
    this.usersSubject.next([newUser, ...this.usersSubject.value]);
  }
}
