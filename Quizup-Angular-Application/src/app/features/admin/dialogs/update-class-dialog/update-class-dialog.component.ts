import { Component, Inject, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ClassService } from '../../../class/services/class.service';
import { SubjectService } from '../../../subject/services/subject.service';
import { SubjectModel } from '../../../subject/models/subject.model';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ClassModel } from '../../../class/models/class.model';

@Component({
  selector: 'app-update-class-dialog',
  templateUrl: './update-class-dialog.component.html',
  styleUrls: ['./update-class-dialog.component.css'],
  imports: [FormsModule, ReactiveFormsModule, CommonModule],
})
export class UpdateClassDialogComponent implements OnInit {
  availableSubjects: SubjectModel[] = [];
  classSubjects: SubjectModel[] = [];
  classForm: FormGroup;
  removeSubjectIds: number[] = [];
  addSubjectIds: number[] = [];

  constructor(
    public dialogRef: MatDialogRef<UpdateClassDialogComponent>,
    private classService: ClassService,
    private subjectService: SubjectService,
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA)
    public data: ClassModel
  ) {
    this.classForm = this.fb.group({
      className: [data.name, Validators.required],
      subjectIds: this.fb.array(
        data.classSubjects!.map((s) => s.subjectId),
        Validators.required
      ),
    });

    this.classSubjects = data.classSubjects!.map((s) => s.subject);
    this.loadSubjects();
  }

  ngOnInit(): void {}

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

  getSubjectName(id: number): string {
    const found = this.availableSubjects.find((s) => s.id === id);
    return found ? found.name : '';
  }

  onSave(): void {
    if (this.classForm.valid) {
      const updatedClass = {
        id: this.data.id,
        className: this.classForm.value.className,
        addSubjectIds: this.addSubjectIds,
        removeSubjectIds: this.removeSubjectIds,
      };

      this.classService.updateClass(updatedClass).subscribe({
        next: (response) => {
          console.log('Class updated:', response);
          this.dialogRef.close(response.data);
        },
        error: (err) => {
          console.error('Error updating class:', err);
        },
      });
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
