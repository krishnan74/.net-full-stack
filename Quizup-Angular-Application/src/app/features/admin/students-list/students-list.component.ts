import { Component, OnInit } from '@angular/core';
import { StudentService } from '../../student/services/student.service';
import { StudentModel } from '../../student/models/student.model';
import { CommonModule } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../dialogs/confirm-dialog/confirm-dialog.component';
import { BreadcrumbsComponent } from '../../../shared/components/breadcrumbs/breadcrumbs.component';
import { UpdateStudentDialogComponent } from '../dialogs/update-student-dialog/update-student-dialog.component';
@Component({
  selector: 'app-students-list',
  templateUrl: './students-list.component.html',
  imports: [CommonModule, BreadcrumbsComponent],
  styleUrls: ['./students-list.component.css'],
})
export class StudentsListComponent implements OnInit {
  students: StudentModel[] = [];

  constructor(
    private studentService: StudentService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadStudents();
  }

  loadStudents(): void {
    this.studentService.getAllStudents().subscribe((data) => {
      this.students = data.data;
      console.log('Students loaded:', this.students);
    });
  }

  updateStudent(student: StudentModel): void {
    const dialogRef = this.dialog.open(UpdateStudentDialogComponent, {
      data: { student },
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.studentService.updateStudent(result).subscribe(() => {
          this.loadStudents();
        });
      }
    });
  }

  deleteStudent(studentId: number): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: { message: 'Are you sure you want to delete this student?' },
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.studentService.deleteStudent(studentId).subscribe(() => {
          this.loadStudents();
        });
      }
    });
  }
}
