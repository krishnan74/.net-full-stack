import { Component } from '@angular/core';
import { ProfileService } from './profile.service';
import { CommonModule } from '@angular/common';
import {
  FormsModule,
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ClassService } from '../class/services/class.service';
import { Store } from '@ngrx/store';
import { User } from '../../store/auth/auth.model';
import { selectUser } from '../../store/auth/state/auth.selectors';
import { Observable, of, switchMap } from 'rxjs';
import { SubjectModel } from '../subject/models/subject.model';
import { ClassModel } from '../class/models/class.model';
import { ToastService } from '../../core/services/toast.service';

@Component({
  selector: 'app-profile',
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './profile.html',
  styleUrls: ['./profile.css'],
})
export class ProfileComponent {
  user$ = this.store.select(selectUser);
  user: User | null = null;

  passwordForm: FormGroup;

  constructor(
    private profileService: ProfileService,
    private store: Store,
    private fb: FormBuilder,
    private toastService: ToastService
  ) {
    this.passwordForm = this.fb.group({
      currentPassword: ['', Validators.required],
      newPassword: ['', Validators.required],
      confirmNewPassword: ['', Validators.required],
    });

    this.studentSubjects$.subscribe({
      next: (subjects) => {
        console.log('Fetched subjects:', subjects);
      },
      error: (error) => {
        console.error('Error loading student subjects:', error);
        this.toastService.showError('Loading Subjects', error);
      },
    });

    this.teacherClasses$.subscribe({
      next: (classes) => {
        console.log('Fetched classes:', classes);
      },
      error: (error) => {
        console.error('Error loading teacher classes:', error);
        this.toastService.showError('Loading Classes', error);
      },
    });

    this.teacherSubjects$.subscribe({
      next: (subjects) => {
        console.log('Fetched teacher subjects:', subjects);
      },
      error: (error) => {
        console.error('Error loading teacher subjects:', error);
        this.toastService.showError('Loading Subjects', error);
      },
    });
  }

  changePassword() {
    if (this.passwordForm.valid) {
      const { currentPassword, newPassword, confirmNewPassword } =
        this.passwordForm.value;

      if (newPassword !== confirmNewPassword) {
        this.toastService.showValidationError(
          'Password',
          'New password and confirm password do not match'
        );
        return;
      }

      if (newPassword.length < 6) {
        this.toastService.showValidationError(
          'Password',
          'New password must be at least 6 characters long'
        );
        return;
      }

      this.profileService
        .changePassword(currentPassword, newPassword)
        .subscribe({
          next: () => {
            console.log('Password changed successfully');
            this.toastService.showSuccess(
              'Password Changed',
              'Password updated successfully'
            );
            this.passwordForm.reset();
          },
          error: (err) => {
            console.error('Error changing password:', err);
            this.toastService.showError('Password Change', err);
          },
        });
    } else {
      this.toastService.showValidationError(
        'Password Form',
        'Please fill in all required fields correctly'
      );
      this.passwordForm.markAllAsTouched();
    }
  }

  studentSubjects$: Observable<SubjectModel[] | null> = this.user$.pipe(
    switchMap((user) =>
      user?.role === 'Student'
        ? this.profileService.getSubjectsByStudentId(user.userId)
        : of(null)
    )
  );

  teacherSubjects$: Observable<SubjectModel[] | null> = this.user$.pipe(
    switchMap((user) =>
      user?.role === 'Teacher'
        ? this.profileService.getSubjectsByTeacherId(user.userId)
        : of(null)
    )
  );

  teacherClasses$: Observable<ClassModel[] | null> = this.user$.pipe(
    switchMap((user) =>
      user?.role === 'Teacher'
        ? this.profileService.getClassesByTeacherId(user.userId)
        : of(null)
    )
  );
}
