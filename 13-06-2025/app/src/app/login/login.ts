import { Component } from '@angular/core';
import { UserModel } from '../models/user';
import { UserService } from '../services/user.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
user:UserModel = new UserModel();
constructor(private userService:UserService){

}
handleLogin(){
  this.userService.login(this.user.username, this.user.password);
}
}

