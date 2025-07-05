import { Component, OnInit } from '@angular/core';
import { SubjectService } from '../../subject/services/subject.service';
import { CommonModule } from '@angular/common';
import { SubjectModel } from '../../subject/models/subject';
import { MatDialog } from '@angular/material/dialog';
import { AddSubjectDialogComponent } from '../dialogs/add-subject-dialog/add-subject-dialog.component';

@Component({
  selector: 'app-subjects-list',
  templateUrl: './subjects-list.component.html',
  imports: [CommonModule],
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
}
