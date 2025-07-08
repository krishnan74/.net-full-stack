import { Component } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { ClassService } from '../../../class/services/class.service';
import { SubjectService } from '../../../subject/services/subject.service';
import { SubjectModel } from '../../../subject/models/subject.model';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-add-class-dialog',
  templateUrl: './add-class-dialog.component.html',
  styleUrls: ['./add-class-dialog.component.css'],
  imports: [FormsModule, ReactiveFormsModule, CommonModule],
})
export class AddClassDialogComponent {
  availableSubjects: SubjectModel[] = [];
  classForm: FormGroup;

  constructor(
    public dialogRef: MatDialogRef<AddClassDialogComponent>,
    private classService: ClassService,
    private subjectService: SubjectService,
    private fb: FormBuilder
  ) {
    this.classForm = this.fb.group({
      className: ['', Validators.required],
      subjectIds: this.fb.array([], Validators.required),
    });

    this.loadSubjects();
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

  get subjectsArray(): FormArray {
    return this.classForm.get('subjectIds') as FormArray;
  }

  addSubject(event: Event): void {
    const value = (event.target as HTMLSelectElement).value;
    if (value && !this.subjectsArray.value.includes(value)) {
      this.subjectsArray.push(this.fb.control(value));
    }
  }

  removeSubject(index: number): void {
    this.subjectsArray.removeAt(index);
  }

  onSave(): void {
    if (this.classForm.valid) {
      this.classService.createClass(this.classForm.value).subscribe({
        next: (response) => {
          console.log('Class created:', response);
          this.dialogRef.close(response.data);
        },
        error: (err) => {
          console.error('Error creating class:', err);
        },
      });
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  getSubjectName(id: number): string {
    const found = this.availableSubjects.find((s) => s.id == id);
    return found ? found.name : '';
  }
}
