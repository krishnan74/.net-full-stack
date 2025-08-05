import { Component, OnInit } from '@angular/core';
import { StudentService } from '../../student/services/student.service';
import { StudentModel } from '../../student/models/student.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../dialogs/confirm-dialog/confirm-dialog.component';
import { BreadcrumbsComponent } from '../../../shared/components/breadcrumbs/breadcrumbs.component';
import { UpdateStudentDialogComponent } from '../dialogs/update-student-dialog/update-student-dialog.component';
import { ToastService } from '../../../core/services/toast.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-students-list',
  templateUrl: './students-list.component.html',
  imports: [CommonModule, FormsModule, BreadcrumbsComponent],
  styleUrls: ['./students-list.component.css'],
})
export class StudentsListComponent implements OnInit {
  students: StudentModel[] = [];
  filteredStudents: StudentModel[] = [];
  searchTerm: string = '';

  constructor(
    private studentService: StudentService,
    private dialog: MatDialog,
    private toastService: ToastService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadStudents();
  }

  loadStudents(): void {
    this.studentService.getAllStudents().subscribe({
      next: (data) => {
        this.students = data.data;
        this.filteredStudents = [...this.students];
        console.log('Students loaded:', this.students);
      },
      error: (error) => {
        console.error('Error loading students:', error);
        this.toastService.showError('Loading Students', error);
      },
    });
  }

  filterStudents(): void {
    if (!this.searchTerm.trim()) {
      this.filteredStudents = [...this.students];
      return;
    }

    const searchLower = this.searchTerm.toLowerCase();
    this.filteredStudents = this.students.filter(
      (student) =>
        student.firstName.toLowerCase().includes(searchLower) ||
        student.lastName.toLowerCase().includes(searchLower) ||
        student.email.toLowerCase().includes(searchLower) ||
        (student.classe?.name &&
          student.classe.name.toLowerCase().includes(searchLower))
    );
  }

  addStudent(): void {
    // This would typically open a dialog to add a new student
    this.router.navigate(['/auth/register']);
  }

  updateStudent(student: StudentModel): void {
    const dialogRef = this.dialog.open(UpdateStudentDialogComponent, {
      data: student,
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.studentService.updateStudent(result).subscribe({
          next: () => {
            this.loadStudents();
            this.toastService.showSuccess(
              'Student Updated',
              `Student ${result.firstName} ${result.lastName} updated successfully`
            );
          },
          error: (error) => {
            console.error('Error updating student:', error);
            this.toastService.showError('Update Student', error);
          },
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
        this.studentService.deleteStudent(studentId).subscribe({
          next: () => {
            this.loadStudents();
            this.toastService.showSuccess(
              'Student Deleted',
              'Student deleted successfully'
            );
          },
          error: (error) => {
            console.error('Error deleting student:', error);
            this.toastService.showError('Delete Student', error);
          },
        });
      }
    });
  }
}
