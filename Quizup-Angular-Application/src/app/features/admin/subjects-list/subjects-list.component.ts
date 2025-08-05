import { Component, OnInit } from '@angular/core';
import { SubjectService } from '../../subject/services/subject.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SubjectModel } from '../../subject/models/subject.model';
import { MatDialog } from '@angular/material/dialog';
import { AddSubjectDialogComponent } from '../dialogs/add-subject-dialog/add-subject-dialog.component';
import { ConfirmDialogComponent } from '../dialogs/confirm-dialog/confirm-dialog.component';
import { BreadcrumbsComponent } from '../../../shared/components/breadcrumbs/breadcrumbs.component';
import { ToastService } from '../../../core/services/toast.service';

@Component({
  selector: 'app-subjects-list',
  templateUrl: './subjects-list.component.html',
  imports: [CommonModule, FormsModule, BreadcrumbsComponent],
  styleUrls: ['./subjects-list.component.css'],
})
export class SubjectsListComponent implements OnInit {
  subjects: SubjectModel[] = [];
  filteredSubjects: SubjectModel[] = [];
  searchTerm: string = '';

  constructor(
    private subjectService: SubjectService,
    private dialog: MatDialog,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.loadSubjects();
  }

  loadSubjects(): void {
    this.subjectService.getAllSubjects().subscribe({
      next: (data) => {
        this.subjects = data.data;
        this.filteredSubjects = [...this.subjects];
      },
      error: (error) => {
        console.error('Error loading subjects:', error);
        this.toastService.showError('Loading Subjects', error);
      },
    });
  }

  filterSubjects(): void {
    if (!this.searchTerm.trim()) {
      this.filteredSubjects = [...this.subjects];
      return;
    }

    const searchLower = this.searchTerm.toLowerCase();
    this.filteredSubjects = this.subjects.filter(
      (subject) =>
        subject.name.toLowerCase().includes(searchLower) ||
        subject.code.toLowerCase().includes(searchLower)
    );
  }

  addOrUpdateSubject(subjectData: SubjectModel | null): void {
    const dialogRef = this.dialog.open(AddSubjectDialogComponent, {
      data: subjectData,
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        console.log('Subject added/updated:', result);
        this.loadSubjects(); // Refresh the subjects list
        if (subjectData) {
          this.toastService.showSuccess(
            'Subject Updated',
            `Subject "${result.name}" updated successfully`
          );
        } else {
          this.toastService.showSuccess(
            'Subject Added',
            `Subject "${result.name}" added successfully`
          );
        }
      }
    });
  }

  deleteSubject(subjectId: number): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: { message: 'Are you sure you want to delete this subject?' },
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.subjectService.deleteSubject(subjectId).subscribe({
          next: () => {
            this.loadSubjects();
            this.toastService.showSuccess(
              'Subject Deleted',
              'Subject deleted successfully'
            );
          },
          error: (error) => {
            console.error('Error deleting subject:', error);
            this.toastService.showError('Delete Subject', error);
          },
        });
      }
    });
  }
}
