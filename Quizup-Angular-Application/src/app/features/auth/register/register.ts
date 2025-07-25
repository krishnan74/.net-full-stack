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
import { SubjectService } from '../../subject/services/subject.service';
import { SubjectModel } from '../../subject/models/subject.model';
import { ClassService } from '../../class/services/class.service';
import { ClassModel } from '../../class/models/class.model';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class RegisterComponent {
  userForm: FormGroup;

  availableSubjects: SubjectModel[] = [];
  availableClasses: ClassModel[] = [];

  constructor(
    private fb: FormBuilder,
    private teacherService: TeacherService,
    private studentService: StudentService,
    private subjectService: SubjectService,
    private classService: ClassService
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

    this.loadAvailableSubjects();
    this.loadAvailableClasses();

    this.userForm.get('role')?.valueChanges.subscribe((role) => {
      if (role === 'Student') {
        this.userForm.get('subjectIds')?.clearValidators();
        this.userForm.get('classIds')?.setValidators([Validators.required]);
      } else if (role === 'Teacher') {
        this.userForm.get('subjectIds')?.setValidators([Validators.required]);
        this.userForm.get('classIds')?.setValidators([Validators.required]);
      }
      this.userForm.get('subjectIds')?.updateValueAndValidity();
      this.userForm.get('classIds')?.updateValueAndValidity();
    });
  }

  loadAvailableSubjects() {
    this.subjectService.getAllSubjects().subscribe({
      next: (response) => {
        this.availableSubjects = response.data;
        console.log('Available subjects:', this.availableSubjects);
      },
      error: (err) => console.error('Error loading subjects:', err),
    });
  }

  loadAvailableClasses() {
    this.classService.getAllClasses().subscribe({
      next: (response) => {
        this.availableClasses = response.data;
        console.log('Available classes:', this.availableClasses);
      },
      error: (err) => console.error('Error loading classes:', err),
    });
  }

  get subjectsArray() {
    return this.userForm.get('subjectIds') as FormArray;
  }

  get classesArray() {
    return this.userForm.get('classIds') as FormArray;
  }

  addSubject(event: Event) {
    console.log('Adding subject');
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
      if (this.userForm.value.role === 'Student') {
        this.classesArray.clear();
        this.classesArray.push(this.fb.control(value));
      } else {
        this.classesArray.push(this.fb.control(value));
      }
    }

    console.log(this.classesArray.value);
  }

  removeClass(index: number) {
    this.classesArray.removeAt(index);
  }

  onSubmit() {
    console.log('Form submitted:', this.userForm.value);

    if (this.userForm.valid) {
      console.log('Form is valid');
      if (this.userForm.value.role === 'Teacher') {
        const createTeacherData = {
          email: this.userForm.value.email,
          password: this.userForm.value.password,
          firstName: this.userForm.value.firstName,
          lastName: this.userForm.value.lastName,
          subjectIds: this.subjectsArray.value.map(Number),
          classIds: this.classesArray.value.map(Number),
        };
        this.teacherService.createTeacher(createTeacherData).subscribe({
          next: (response) => console.log('Teacher created:', response),
          error: (err) => console.error('Error:', err),
        });
      }
      else if(this.userForm.value.role === 'Student') {

        const createStudentData = {
          email: this.userForm.value.email,
          password: this.userForm.value.password,
          firstName: this.userForm.value.firstName,
          lastName: this.userForm.value.lastName,
          classId: Number(this.classesArray.value[0])
        };

        console.log('Creating student with data:', createStudentData);
        this.studentService.createStudent(createStudentData).subscribe({
          next: (response) => console.log('Student created:', response),
          error: (err) => console.error('Error:', err),
        });
      }
    } else {
      console.log('Form is invalid');
      this.userForm.markAllAsTouched();
    }
  }

  getClassName(id: number): string {
    const found = this.availableClasses.find((c) => c.id == id);
    return found ? found.name : '';
  }

  getSubjectName(id: number): string {
    const found = this.availableSubjects.find((s) => s.id == id);
    return found ? found.name : '';
  }
}
