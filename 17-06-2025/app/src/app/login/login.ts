import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UserModel } from '../models/user';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-login',
  imports: [],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  user:UserModel = new UserModel();

  constructor(private userService: UserService, private router: Router) {}

  handleLogin() {
    this.userService.login(this.user.username, this.user.password)
    if (this.user.username) {
      this.router.navigateByUrl(`/home/${this.user.username}`);
    } else {
      alert('Please enter a username');
    }
  }

}