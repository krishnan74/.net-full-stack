import { Injectable } from "@angular/core";
import { UserModel } from "../models/user";

@Injectable()
export class UserService {
    private dummyUsers: UserModel[] = [
        new UserModel('testuser', 'password123'),
        new UserModel('admin', 'adminpass')
    ];

    login(username: string, password: string): void {
        const user = this.dummyUsers.find(
            u => u.username === username && u.password === password
        );
        if (user) {
            localStorage.setItem('user', JSON.stringify(user));
        }
    }

    logout(): void {
        localStorage.removeItem('user');
    }

    getCurrentUser(): UserModel | null {
        const userJson = localStorage.getItem('user');
        return userJson ? JSON.parse(userJson) : null;
    }
}
