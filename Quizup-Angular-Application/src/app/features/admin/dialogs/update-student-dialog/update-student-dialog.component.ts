import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { StudentModel } from '../../../student/models/student.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-update-student-dialog',
  templateUrl: './update-student-dialog.component.html',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
})
export class UpdateStudentDialogComponent {
  form: FormGroup;

  constructor(
    public dialogRef: MatDialogRef<UpdateStudentDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { student: StudentModel },
    private fb: FormBuilder
  ) {
    this.form = this.fb.group({
      firstName: [data.student.firstName, Validators.required],
      lastName: [data.student.lastName, Validators.required],
      email: [data.student.email, [Validators.required, Validators.email]],
    });
  }

  onSubmit() {
    if (this.form.valid) {
      this.dialogRef.close({ ...this.data.student, ...this.form.value });
    }
  }

  onCancel() {
    this.dialogRef.close();
  }
}
