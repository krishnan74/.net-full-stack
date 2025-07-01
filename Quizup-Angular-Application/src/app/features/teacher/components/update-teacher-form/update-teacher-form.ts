import { Component } from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { Store } from '@ngrx/store';
import { CommonModule } from '@angular/common';
import { TeacherService } from '../../services/teacher.service';

@Component({
  selector: 'app-update-teacher-form',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './update-teacher-form.html',
  styleUrl: './update-teacher-form.css'
})
export class UpdateTeacherForm {
userForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private teacherService: TeacherService,
  ) {
    this.userForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(3)]],
      lastName: ['', [Validators.required, Validators.minLength(3)]],
      currentPassword: ['', [Validators.required, Validators.minLength(6)]],
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
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
