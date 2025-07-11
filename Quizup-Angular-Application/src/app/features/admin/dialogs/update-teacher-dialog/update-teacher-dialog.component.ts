import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import {
  FormArray,
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { TeacherModel } from '../../../teacher/models/teacher.model';
import { CommonModule } from '@angular/common';
import { TeacherService } from '../../../teacher/services/teacher.service';
import { TeacherSubjectModel } from '../../../../shared/models/teacher-subject.model';
import { TeacherClassModel } from '../../../../shared/models/teacher-class.model';

import { SubjectModel } from '../../../subject/models/subject.model';
import { ClassModel } from '../../../class/models/class.model';
import { ClassService } from '../../../class/services/class.service';
import { SubjectService } from '../../../subject/services/subject.service';

@Component({
  selector: 'app-update-teacher-dialog',
  templateUrl: './update-teacher-dialog.component.html',
  styleUrls: ['./update-teacher-dialog.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
})
export class UpdateTeacherDialogComponent implements OnInit {
  teacherUpdateForm: FormGroup;

  availableSubjects: SubjectModel[] = [];
  availableClasses: ClassModel[] = [];

  ngOnInit(): void {}

  teacherSubjects: SubjectModel[] = [];
  teacherClasses: ClassModel[] = [];

  removeSubjectIds: number[] = [];
  addSubjectIds: number[] = [];
  removeClassIds: number[] = [];
  addClassIds: number[] = [];

  constructor(
    public dialogRef: MatDialogRef<UpdateTeacherDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: TeacherModel,
    private fb: FormBuilder,
    private teacherService: TeacherService,
    private subjectService: SubjectService,
    private classService: ClassService
  ) {
    this.teacherUpdateForm = this.fb.group({
      firstName: [data.firstName, Validators.required],
      lastName: [data.lastName, Validators.required],
      subjectIds: this.fb.array(
        data.teacherSubjects!.map((s) => s.subjectId),
        Validators.required
      ),
      classIds: this.fb.array(
        data.teacherClasses!.map((c) => c.classId),
        Validators.required
      ),
    });

    this.teacherClasses = data.teacherClasses!.map((c) => c.classe);
    this.teacherSubjects = data.teacherSubjects!.map((s) => s.subject);
    this.loadSubjects();
    this.loadClasses();
  }

  loadSubjects(): void {
    this.subjectService.getAllSubjects().subscribe({
      next: (response) => {
        this.availableSubjects = response.data;
      },
      error: (err) => {
        console.error('Error loading subjects:', err);
      },
    });
  }

  loadClasses(): void {
    this.classService.getAllClasses().subscribe({
      next: (response) => {
        this.availableClasses = response.data;
      },
      error: (err) => {
        console.error('Error loading classes:', err);
      },
    });
  }

  get subjectsArray(): FormArray {
    return this.teacherUpdateForm.get('subjectIds') as FormArray;
  }

  get classesArray(): FormArray {
    return this.teacherUpdateForm.get('classIds') as FormArray;
  }

  addSubject(event: Event): void {
    const value = parseInt((event.target as HTMLSelectElement).value, 10);
    if (value && !this.subjectsArray.value.includes(value)) {
      this.subjectsArray.push(this.fb.control(value));
      this.addSubjectIds.push(value);
    }
  }

  removeSubject(index: number): void {
    const removedId = this.subjectsArray.at(index).value;
    this.subjectsArray.removeAt(index);
    this.removeSubjectIds.push(removedId);
  }

  addClass(event: Event): void {
    const value = parseInt((event.target as HTMLSelectElement).value, 10);
    if (value && !this.classesArray.value.includes(value)) {
      this.classesArray.push(this.fb.control(value));
      this.addClassIds.push(value);
    }
  }

  removeClass(index: number): void {
    const removedId = this.classesArray.at(index).value;
    this.classesArray.removeAt(index);
    this.removeClassIds.push(removedId);
  }

  getSubjectName(id: number): string {
    const found = this.availableSubjects.find((s) => s.id === id);
    return found ? found.name : '';
  }

  getClassName(id: number): string {
    const found = this.availableClasses.find((c) => c.id === id);
    return found ? found.name : '';
  }

  onSave(): void {
    if (this.teacherUpdateForm.valid) {
      const updatedTeacher = {
        id: this.data.id,
        firstName: this.teacherUpdateForm.value.firstName,
        lastName: this.teacherUpdateForm.value.lastName,
        addSubjectIds: this.addSubjectIds,
        removeSubjectIds: this.removeSubjectIds,
        addClassIds: this.addClassIds,
        removeClassIds: this.removeClassIds,
      };

      this.teacherService.updateTeacher(updatedTeacher).subscribe({
        next: (response) => {
          console.log('Teacher updated:', response);
          this.dialogRef.close(response.data);
        },
        error: (err) => {
          console.error('Error updating teacher:', err);
        },
      });
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
