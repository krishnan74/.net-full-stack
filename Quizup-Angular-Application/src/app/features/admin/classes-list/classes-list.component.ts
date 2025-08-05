import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

import { ClassService } from '../../class/services/class.service';
import { ClassModel } from '../../class/models/class.model';
import { AddSubjectDialogComponent } from '../dialogs/add-subject-dialog/add-subject-dialog.component';
import { AddClassDialogComponent } from '../dialogs/add-class-dialog/add-class-dialog.component';
import { CommonModule } from '@angular/common';
import { UpdateClassDialogComponent } from '../dialogs/update-class-dialog/update-class-dialog.component';
import { ConfirmDialogComponent } from '../dialogs/confirm-dialog/confirm-dialog.component';
import { BreadcrumbsComponent } from '../../../shared/components/breadcrumbs/breadcrumbs.component';
import { ToastService } from '../../../core/services/toast.service';

@Component({
  selector: 'app-classes-list',
  templateUrl: './classes-list.component.html',
  imports: [CommonModule, BreadcrumbsComponent],
  styleUrls: ['./classes-list.component.css'],
})
export class ClassesListComponent implements OnInit {
  classes: ClassModel[] = [];

  constructor(
    private classService: ClassService,
    private dialog: MatDialog,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.loadClasses();
  }

  loadClasses(): void {
    this.classService.getAllClasses().subscribe({
      next: (data) => {
        this.classes = data.data;
        console.log('Classes loaded:', this.classes);
      },
      error: (error) => {
        console.error('Error loading classes:', error);
        this.toastService.showError('Loading Classes', error);
      },
    });
  }

  addClass(): void {
    const dialogRef = this.dialog.open(AddClassDialogComponent, {});

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        console.log('Class added/updated:', result);
        this.loadClasses();
        this.toastService.showSuccess(
          'Class Added',
          `Class "${result.name}" added successfully`
        );
      }
    });
  }

  updateClass(classe: ClassModel): void {
    const dialogRef = this.dialog.open(UpdateClassDialogComponent, {
      data: classe,
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        console.log('Class updated:', result);
        this.loadClasses();
        this.toastService.showSuccess(
          'Class Updated',
          `Class "${result.name}" updated successfully`
        );
      }
    });
  }

  deleteClass(classId: number): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: { message: 'Are you sure you want to delete this class?' },
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        console.log('Deleting class with ID:', classId);
        this.classService.deleteClass(classId).subscribe({
          next: () => {
            this.loadClasses();
            this.toastService.showSuccess(
              'Class Deleted',
              'Class deleted successfully'
            );
          },
          error: (error) => {
            console.error('Error deleting class:', error);
            this.toastService.showError('Delete Class', error);
          },
        });
      }
    });
  }
}
