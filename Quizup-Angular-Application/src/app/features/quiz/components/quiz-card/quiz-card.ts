import { Component, Input } from '@angular/core';
import { Quiz } from '../../models/quiz';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-quiz-card',
  imports: [CommonModule],
  templateUrl: './quiz-card.html',
  styleUrl: './quiz-card.css',
})
export class QuizCard {
  @Input() quiz: Quiz | null = new Quiz();
  @Input() showAttempt: boolean = true;

  viewResults() {}

  startQuiz() {}
}
