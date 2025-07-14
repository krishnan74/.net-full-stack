import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-quiz-result-dialog',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './quiz-result-dialog.html',
  styleUrl: './quiz-result-dialog.css',
})
export class QuizResultDialog {
  @Input() score: number = 0;
  @Input() visible: boolean = false;
  @Output() reviewAnswers = new EventEmitter<void>();
  @Output() goBackToQuizzes = new EventEmitter<void>();

  handleReview() {
    this.reviewAnswers.emit();
  }

  handleGoBack() {
    this.goBackToQuizzes.emit();
  }
}
