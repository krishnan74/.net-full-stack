import { Component, OnInit } from '@angular/core';
import { SubjectService } from '../../subject/services/subject.service';
import { CommonModule } from '@angular/common';
import { SubjectModel } from '../../subject/models/subject.model';
import { MatDialog } from '@angular/material/dialog';
import { AddSubjectDialogComponent } from '../dialogs/add-subject-dialog/add-subject-dialog.component';
import { ConfirmDialogComponent } from '../dialogs/confirm-dialog/confirm-dialog.component';
import { BreadcrumbsComponent } from '../../../shared/components/breadcrumbs/breadcrumbs.component';
@Component({
  selector: 'app-subjects-list',
  templateUrl: './subjects-list.component.html',
  imports: [CommonModule, BreadcrumbsComponent],
  styleUrls: ['./subjects-list.component.css'],
})
export class SubjectsListComponent implements OnInit {
  subjects: SubjectModel[] = [];

  constructor(
    private subjectService: SubjectService,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.loadSubjects();
  }

  loadSubjects(): void {
    this.subjectService.getAllSubjects().subscribe((data) => {
      this.subjects = data.data;
    });
  }

  addOrUpdateSubject(subjectData: SubjectModel | null): void {
    const dialogRef = this.dialog.open(AddSubjectDialogComponent, {
      data: subjectData,
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        console.log('Subject added/updated:', result);
        this.loadSubjects(); // Refresh the subjects list
      }
    });
  }

  deleteSubject(subjectId: number): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: { message: 'Are you sure you want to delete this subject?' },
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.subjectService.deleteSubject(subjectId).subscribe(() => {
          this.loadSubjects();
        });
      }
    });
  }
}
