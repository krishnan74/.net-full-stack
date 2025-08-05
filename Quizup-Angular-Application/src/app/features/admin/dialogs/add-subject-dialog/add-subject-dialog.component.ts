import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { SubjectService } from '../../../subject/services/subject.service';
import { ToastService } from '../../../../core/services/toast.service';

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
    private subjectService: SubjectService,
    private toastService: ToastService
  ) {}

  onSave(): void {
    if (!this.subjectName.trim() || !this.subjectCode.trim()) {
      this.toastService.showValidationError(
        'Subject Form',
        'Please fill in all required fields'
      );
      return;
    }

    const newSubject = { name: this.subjectName, code: this.subjectCode };
    this.subjectService.createSubject(newSubject).subscribe({
      next: (response) => {
        console.log('Subject created:', response);
        this.toastService.showSuccess(
          'Subject Created',
          `Subject "${this.subjectName}" created successfully`
        );
        this.dialogRef.close(response.data);
      },
      error: (err) => {
        console.error('Error creating subject:', err);
        this.toastService.showError('Create Subject', err);
      },
    });
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
