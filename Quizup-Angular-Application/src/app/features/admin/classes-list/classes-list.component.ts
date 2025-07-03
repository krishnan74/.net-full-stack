import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

import { ClassService } from '../../class/services/class.service';
import { ClassModel } from '../../class/models/class';
import { AddSubjectDialogComponent } from '../dialogs/add-subject-dialog/add-subject-dialog.component';
import { AddClassDialogComponent } from '../dialogs/add-class-dialog/add-class-dialog.component';
import { CommonModule } from '@angular/common';
import { UpdateClassDialogComponent } from '../dialogs/update-class-dialog/update-class-dialog.component';

@Component({
  selector: 'app-classes-list',
  templateUrl: './classes-list.component.html',
  imports: [CommonModule],
  styleUrls: ['./classes-list.component.css'],
})
export class ClassesListComponent implements OnInit {
  classes: ClassModel[] = [];

  constructor(private classService: ClassService, private dialog: MatDialog) {}

  ngOnInit(): void {
    this.loadClasses();
  }

  loadClasses(): void {
    this.classService.getAllClasses().subscribe((data) => {
      this.classes = data.data;
      console.log('Classes loaded:', this.classes);
    });
  }

  addClass(): void {
    const dialogRef = this.dialog.open(AddClassDialogComponent, {});

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        console.log('Class added/updated:', result);
        // Logic to add or update class
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
        // Logic to update class
      }
    });
  }
}
