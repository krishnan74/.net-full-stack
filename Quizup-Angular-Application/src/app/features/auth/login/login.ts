// src/app/features/auth/login/login.component.ts

import { Component } from '@angular/core';
import { Store } from '@ngrx/store';
import { login } from '../../../store/auth/state/auth.actions';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css'],
})
export class LoginComponent {
  userForm: FormGroup;

  constructor(private fb: FormBuilder, private store: Store) {
    this.userForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  onSubmit() {
    if (this.userForm.valid) {
      this.store.dispatch(login({ payload: this.userForm.value }));
    } else {
      this.userForm.markAllAsTouched();
    }
  }
}
