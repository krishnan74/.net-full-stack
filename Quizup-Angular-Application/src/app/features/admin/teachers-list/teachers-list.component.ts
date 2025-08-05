import { Component, OnInit } from '@angular/core';
import { TeacherService } from '../../teacher/services/teacher.service';
import { TeacherModel } from '../../teacher/models/teacher.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../dialogs/confirm-dialog/confirm-dialog.component';
import { BreadcrumbsComponent } from '../../../shared/components/breadcrumbs/breadcrumbs.component';
import { UpdateTeacherDialogComponent } from '../dialogs/update-teacher-dialog/update-teacher-dialog.component';
import { ToastService } from '../../../core/services/toast.service';

@Component({
  selector: 'app-teachers-list',
  templateUrl: './teachers-list.component.html',
  imports: [CommonModule, FormsModule, BreadcrumbsComponent],
  styleUrls: ['./teachers-list.component.css'],
})
export class TeachersListComponent implements OnInit {
  teachers: TeacherModel[] = [];
  filteredTeachers: TeacherModel[] = [];
  searchTerm: string = '';

  constructor(
    private teacherService: TeacherService,
    private dialog: MatDialog,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.loadTeachers();
  }

  loadTeachers(): void {
    this.teacherService.getAllTeachers().subscribe({
      next: (data) => {
        this.teachers = data.data;
        this.filteredTeachers = [...this.teachers];
        console.log('Teachers loaded:', this.teachers);
      },
      error: (error) => {
        console.error('Error loading teachers:', error);
        this.toastService.showError('Loading Teachers', error);
      },
    });
  }

  filterTeachers(): void {
    if (!this.searchTerm.trim()) {
      this.filteredTeachers = [...this.teachers];
      return;
    }

    const searchLower = this.searchTerm.toLowerCase();
    this.filteredTeachers = this.teachers.filter(
      (teacher) =>
        teacher.firstName.toLowerCase().includes(searchLower) ||
        teacher.lastName.toLowerCase().includes(searchLower) ||
        teacher.email.toLowerCase().includes(searchLower) ||
        teacher.teacherClasses?.some((tc) =>
          tc.classe.name.toLowerCase().includes(searchLower)
        ) ||
        teacher.teacherSubjects?.some((ts) =>
          ts.subject.name.toLowerCase().includes(searchLower)
        )
    );
  }

  addTeacher(): void {
    // This would typically open a dialog to add a new teacher
    this.toastService.showInfo(
      'Add Teacher',
      'Add teacher functionality would be implemented here'
    );
  }

  updateTeacher(teacher: TeacherModel): void {
    const dialogRef = this.dialog.open(UpdateTeacherDialogComponent, {
      data: teacher,
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.teacherService.updateTeacher(result).subscribe({
          next: () => {
            this.loadTeachers();
            this.toastService.showSuccess(
              'Teacher Updated',
              `Teacher ${result.firstName} ${result.lastName} updated successfully`
            );
          },
          error: (error) => {
            console.error('Error updating teacher:', error);
            this.toastService.showError('Update Teacher', error);
          },
        });
      }
    });
  }

  deleteTeacher(teacherId: number): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: { message: 'Are you sure you want to delete this teacher?' },
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.teacherService.deleteTeacher(teacherId).subscribe({
          next: () => {
            this.loadTeachers();
            this.toastService.showSuccess(
              'Teacher Deleted',
              'Teacher deleted successfully'
            );
          },
          error: (error) => {
            console.error('Error deleting teacher:', error);
            this.toastService.showError('Delete Teacher', error);
          },
        });
      }
    });
  }
}
