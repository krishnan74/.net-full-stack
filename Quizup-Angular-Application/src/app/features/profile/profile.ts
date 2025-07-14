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
    private fb: FormBuilder
  ) {
    this.passwordForm = this.fb.group({
      currentPassword: ['', Validators.required],
      newPassword: ['', Validators.required],
      confirmNewPassword: ['', Validators.required],
    });

    this.studentSubjects$.subscribe((subjects) => {
      console.log('Fetched subjects:', subjects);
    });

    this.teacherClasses$.subscribe((classes) => {
      console.log('Fetched classes:', classes);
    });

    this.teacherSubjects$.subscribe((subjects) => {
      console.log('Fetched teacher subjects:', subjects);
    });
  }

  changePassword() {
    if (this.passwordForm.valid) {
      this.profileService
        .changePassword(
          this.passwordForm.value.currentPassword,
          this.passwordForm.value.newPassword
        )
        .subscribe({
          next: () => {
            console.log('Password changed successfully');
          },
          error: (err) => {
            console.error('Error changing password:', err);
          },
        });
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
