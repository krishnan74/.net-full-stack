import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
import { UserModel } from "../models/user";

@Injectable()
export class UserService {

    private dummyUsers: UserModel[] = [
        new UserModel('testuser', 'password123'),
        new UserModel('admin', 'adminpass')
    ];

    private usernameSubject = new BehaviorSubject<string|null>(null);
    username$:Observable<string|null> = this.usernameSubject.asObservable();

    login(username: string, password: string): void {
        const user = this.dummyUsers.find(
            u => u.username === username && u.password === password
        );
        if (user) {
            localStorage.setItem('user', JSON.stringify(user));
            this.usernameSubject.next(user.username);
        }
        else{
            this.usernameSubject.error("Invalid username or password");
        }
    }

    logout(): void {
        localStorage.removeItem('user');
        this.usernameSubject.next(null);
    }

    getCurrentUser(): UserModel | null {
        const userJson = localStorage.getItem('user');
        return userJson ? JSON.parse(userJson) : null;
    }
}
