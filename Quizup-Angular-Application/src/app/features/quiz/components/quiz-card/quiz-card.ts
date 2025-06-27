import { Component, Input } from '@angular/core';
import { QuizModel } from '../../models/quiz.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-quiz-card',
  imports: [CommonModule],
  templateUrl: './quiz-card.html',
  styleUrl: './quiz-card.css',
})
export class QuizCard {
  @Input() quiz: QuizModel | null = new QuizModel();
  @Input() showAttempt: boolean = true;

  viewResults() {}

  startQuiz() {}
}
