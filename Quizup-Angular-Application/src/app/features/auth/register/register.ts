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

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class RegisterComponent {
  userForm: FormGroup;

  constructor(private fb: FormBuilder, private teacherService: TeacherService) {
    this.userForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(3)]],
      lastName: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      role: ['', [Validators.required]],
    });
  }

  onSubmit() {
      console.log("Before validation", this.userForm.value)

    if (this.userForm.valid) {
      console.log("After validation", this.userForm.value)
      const teacher = this.teacherService.createTeacher(this.userForm.value)
      console.log(teacher)
    } else {
      this.userForm.markAllAsTouched();
    }
  }
}
