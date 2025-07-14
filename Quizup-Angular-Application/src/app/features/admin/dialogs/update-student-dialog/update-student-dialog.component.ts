import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { StudentModel } from '../../../student/models/student.model';
import { CommonModule } from '@angular/common';
import { ClassModel } from '../../../class/models/class.model';
import { ClassService } from '../../../class/services/class.service';
import { StudentService } from '../../../student/services/student.service';

@Component({
  selector: 'app-update-student-dialog',
  templateUrl: './update-student-dialog.component.html',
  styleUrls: ['./update-student-dialog.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
})
export class UpdateStudentDialogComponent implements OnInit {
  studentUpdateForm: FormGroup;

  availableClasses: ClassModel[] = [];

  ngOnInit(): void {}

  constructor(
    public dialogRef: MatDialogRef<UpdateStudentDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: StudentModel,
    private fb: FormBuilder,
    private classService: ClassService,
    private studentService: StudentService
  ) {
    this.studentUpdateForm = this.fb.group({
      firstName: [data.firstName, Validators.required],
      lastName: [data.lastName, Validators.required],
      classId: [data.classe.id, Validators.required],
    });
    this.loadClasses();
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

  onSave(): void {
    if (this.studentUpdateForm.valid) {
      const updatedStudent = {
        id: this.data.id,
        firstName: this.studentUpdateForm.value.firstName,
        lastName: this.studentUpdateForm.value.lastName,
        classId: this.studentUpdateForm.value.classId,
      };

      console.log('Updating student:', updatedStudent);

      this.studentService.updateStudent(updatedStudent).subscribe({
        next: (response) => {
          console.log('Student updated:', response);
          this.dialogRef.close(response.data);
        },
        error: (err) => {
          console.error('Error updating student:', err);
        },
      });
    }
  }
  onCancel() {
    this.dialogRef.close();
  }
}
