import { Component } from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { UserService } from '../features/users/users.service';
import { usernameValidator } from '../misc/username-validator';

@Component({
  selector: 'app-user-form',
  imports: [ReactiveFormsModule],
  templateUrl: './user-form.html',
  styleUrl: './user-form.css',
})
export class UserForm {
  userForm: FormGroup;

  constructor(private fb: FormBuilder, private userService: UserService) {
    this.userForm = this.fb.group({
      username: [
        '',
        [Validators.required, Validators.minLength(3), usernameValidator()],
      ],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(6)]],
      role: ['', [Validators.required]],
    });
  }

  onSubmit() {
    console.log('Form Submitted', this.userForm.value);
    if (this.userForm.valid) {
      console.log('Form Submitted', this.userForm.value);

      const userData = this.userForm.value;
      this.userService.addUser(userData);
    } else {
      this.userForm.markAllAsTouched();
    }
  }
}
