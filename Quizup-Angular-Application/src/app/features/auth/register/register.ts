import { Component } from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  Validators,
  ReactiveFormsModule,
  FormArray,
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

  availableSubjects = [{
    id: 1,
    name: 'Mathematics',
  },
  {
    id: 2,
    name: 'Science',
  },
  {
    id: 3,
    name: 'History',
  },
  {
    id: 4,
    name: 'Geography',
  }
];
  availableClasses = [{
    id: 1,
    name: 'X - C',
  },
  {
    id: 2,
    name: 'X - D',
  },
  {
    id: 3,
    name: 'VII - A',
  },
  {
    id: 4,
    name: 'VII - B',
  }];

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
      subjectIds: this.fb.array([], [Validators.required]),
      classIds: this.fb.array([], [Validators.required]),
      role: ['', [Validators.required]],
    });
  }

  get subjectsArray() {
    return this.userForm.get('subjectIds') as FormArray;
  }

  get classesArray() {
    return this.userForm.get('classIds') as FormArray;
  }

  addSubject(event: Event) {
    console.log("Adding subject");
    const value = (event.target as HTMLSelectElement).value;
    if (value && !this.subjectsArray.value.includes(value)) {
      this.subjectsArray.push(this.fb.control(value));
    }
    console.log(this.subjectsArray.value);

  }

  removeSubject(index: number) {
    this.subjectsArray.removeAt(index);
  }

  addClass(event: Event) {
    const value = (event.target as HTMLSelectElement).value;
    if (value && !this.classesArray.value.includes(value)) {
      this.classesArray.push(this.fb.control(value));
    }
  }

  removeClass(index: number) {
    this.classesArray.removeAt(index);
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

  getClassName(id: number): string {
    const found = this.availableClasses.find(c => c.id == id);
    return found ? found.name : '';
  }

  getSubjectName(id: number): string {
    const found = this.availableSubjects.find(s => s.id == id);
    return found ? found.name : '';
  }
}
