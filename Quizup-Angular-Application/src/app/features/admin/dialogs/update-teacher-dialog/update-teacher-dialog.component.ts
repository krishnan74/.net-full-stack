import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { TeacherModel } from '../../../teacher/models/teacher.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-update-teacher-dialog',
  templateUrl: './update-teacher-dialog.component.html',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
})
export class UpdateTeacherDialogComponent {
  form: FormGroup;

  constructor(
    public dialogRef: MatDialogRef<UpdateTeacherDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { teacher: TeacherModel },
    private fb: FormBuilder
  ) {
    this.form = this.fb.group({
      firstName: [data.teacher.firstName, Validators.required],
      lastName: [data.teacher.lastName, Validators.required],
      email: [data.teacher.email, [Validators.required, Validators.email]],
    });
  }

  onSubmit() {
    if (this.form.valid) {
      this.dialogRef.close({ ...this.data.teacher, ...this.form.value });
    }
  }

  onCancel() {
    this.dialogRef.close();
  }
}
