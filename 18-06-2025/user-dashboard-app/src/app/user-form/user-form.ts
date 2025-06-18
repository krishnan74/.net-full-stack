import { Component } from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { UserService } from '../features/users/users.service';

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
      username: ['', [Validators.required, Validators.minLength(3)]],
      firstName: ['', [Validators.required, Validators.minLength(3)]],
      lastName: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      gender: ['', [Validators.required]],
      role: ['', [Validators.required]],
      state: ['', [Validators.required]],
    });
  }

  onSubmit() {
    console.log('Form Submitted', this.userForm.value);
    if (this.userForm.valid) {
      console.log('Form Submitted', this.userForm.value);
      this.userForm.patchValue({
        role: this.userForm.value.role.toLowerCase(),
        state:
          this.userForm.value.state.charAt(0).toUpperCase() +
          this.userForm.value.state.slice(1),
        gender: this.userForm.value.gender.toLowerCase(),
      });

      const userData = this.userForm.value;
      this.userService.addUser(userData);
    } else {
      this.userForm.markAllAsTouched();
    }
  }
}
