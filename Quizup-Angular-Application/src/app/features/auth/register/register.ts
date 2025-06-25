import { Component } from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { Store } from '@ngrx/store';
import { login } from '../../../store/auth/state/auth.actions';
import { CommonModule } from '@angular/common';
import { TeacherService } from '../../teacher/services/teacher.service';
import { StudentService } from '../../student/services/student.service';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class RegisterComponent {
  userForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private teacherService: TeacherService,
    private studentService: StudentService
  ) {
    this.userForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      firstName: ['', [Validators.required, Validators.minLength(3)]],
      lastName: ['', [Validators.required, Validators.minLength(3)]],
      subject: ['', [Validators.required, Validators.minLength(3)]],
      role: ['', [Validators.required]],
    });
  }

  onSubmit() {
    console.log('Form submitted:', this.userForm.value);

    if (this.userForm.valid) {
      console.log('Form is valid');
      if (this.userForm.value.role === 'Teacher') {
        this.teacherService.createTeacher(this.userForm.value).subscribe({
          next: (response) => console.log('Teacher created:', response),
          error: (err) => console.error('Error:', err),
        });
      }
    } else {
      console.log('Form is invalid');
      this.userForm.markAllAsTouched();
    }
  }
}
