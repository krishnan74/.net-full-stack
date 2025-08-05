// src/app/features/auth/login/login.component.ts

import { Component, OnInit, OnDestroy } from '@angular/core';
import { Store } from '@ngrx/store';
import {
  login,
  loginSuccess,
  loginFailure,
} from '../../../store/auth/state/auth.actions';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import {
  selectUser,
  selectAuthError,
} from '../../../store/auth/state/auth.selectors';
import { Subscription, filter } from 'rxjs';
import { ToastService } from '../../../core/services/toast.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css'],
})
export class LoginComponent implements OnInit, OnDestroy {
  userForm: FormGroup;
  user$ = this.store.select(selectUser);
  error$ = this.store.select(selectAuthError);
  private userSubscription: Subscription | undefined;
  private errorSubscription: Subscription | undefined;
  private previousUser: any = null;

  constructor(
    private fb: FormBuilder,
    private store: Store,
    private toastService: ToastService
  ) {
    this.userForm = this.fb.group({
      username: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  ngOnInit() {
    // Subscribe to user changes
    this.userSubscription = this.user$.subscribe((user) => {
      console.log('User state changed:', user);

      // Only show toast if user was null before and is now logged in
      if (user && !this.previousUser) {
        console.log('User logged in successfully:', user);
        this.toastService.showSuccess(
          'Login',
          `Welcome back, ${user.username}!`
        );
      }

      this.previousUser = user;
    });

    // Subscribe to error changes
    this.errorSubscription = this.error$
      .pipe(filter((error) => !!error))
      .subscribe((error) => {
        console.log('Login error:', error);
        if (error) {
          this.toastService.showError('Login Failed', error);
        }
      });
  }

  ngOnDestroy() {
    if (this.userSubscription) {
      this.userSubscription.unsubscribe();
    }
    if (this.errorSubscription) {
      this.errorSubscription.unsubscribe();
    }
  }

  onSubmit() {
    console.log('Form submitted:', this.userForm.value);
    if (this.userForm.valid) {
      console.log('Form is valid, dispatching login action');
      this.store.dispatch(login({ payload: this.userForm.value }));
    } else {
      console.log('Form is invalid, showing validation error');
      this.userForm.markAllAsTouched();
      this.toastService.showValidationError(
        'Form',
        'Please fill in all required fields correctly.'
      );
    }
  }
}
