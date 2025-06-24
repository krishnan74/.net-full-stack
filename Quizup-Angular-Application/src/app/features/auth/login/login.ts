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
import { selectUser } from '../../../store/auth/state/auth.selectors';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css'],
})
export class LoginComponent {
  userForm: FormGroup;
  user$ = this.store.select(selectUser);
  private userSubscription?: Subscription;

  constructor(private fb: FormBuilder, private store: Store) {
    this.userForm = this.fb.group({
      username: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });

    
  }

  onSubmit() {
    console.log('Form went to submit', this.userForm.value);
    if (this.userForm.valid) {
      console.log('Form Submitted!', this.userForm.value);
      this.store.dispatch(login({ payload: this.userForm.value }));
      this.user$.subscribe(user => {
      console.log(user);
    });
    } else {
      this.userForm.markAllAsTouched();
    }
  }
}
