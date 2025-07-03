import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { SubjectService } from '../../../subject/services/subject.service';

@Component({
  selector: 'app-add-subject-dialog',
  templateUrl: './add-subject-dialog.component.html',
  styleUrls: ['./add-subject-dialog.component.css'],
  imports: [FormsModule],
})
export class AddSubjectDialogComponent {
  subjectName: string = '';
  subjectCode: string = '';

  constructor(
    public dialogRef: MatDialogRef<AddSubjectDialogComponent>,
    private subjectService: SubjectService
  ) {}

  onSave(): void {
    const newSubject = { name: this.subjectName, code: this.subjectCode };
    this.subjectService.createSubject(newSubject).subscribe({
      next: (response) => {
        console.log('Subject created:', response);
        this.dialogRef.close(response.data);
      },
      error: (err) => {
        console.error('Error creating subject:', err);
      },
    });
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
