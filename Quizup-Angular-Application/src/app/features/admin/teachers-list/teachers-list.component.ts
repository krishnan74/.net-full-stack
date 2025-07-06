import { Component, OnInit } from '@angular/core';
import { TeacherService } from '../../teacher/services/teacher.service';
import { TeacherModel } from '../../teacher/models/teacher';
import { CommonModule } from '@angular/common';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogComponent } from '../dialogs/confirm-dialog/confirm-dialog.component';
import { BreadcrumbsComponent } from '../../../shared/components/breadcrumbs/breadcrumbs.component';

@Component({
  selector: 'app-teachers-list',
  templateUrl: './teachers-list.component.html',
  imports: [CommonModule, BreadcrumbsComponent],
  styleUrls: ['./teachers-list.component.css'],
})
export class TeachersListComponent implements OnInit {
  teachers: TeacherModel[] = [];

  constructor(
    private teacherService: TeacherService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadTeachers();
  }

  loadTeachers(): void {
    this.teacherService.getAllTeachers().subscribe((data) => {
      this.teachers = data.data;
      console.log('Teachers loaded:', this.teachers);
    });
  }

  updateTeacher(teacher: any): void {
    // Logic to update teacher
  }

  deleteTeacher(teacherId: number): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: { message: 'Are you sure you want to delete this teacher?' },
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.teacherService.deleteTeacher(teacherId).subscribe(() => {
          this.loadTeachers();
        });
      }
    });
  }
}
