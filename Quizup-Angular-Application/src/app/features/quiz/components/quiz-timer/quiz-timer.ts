import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatProgressBarModule } from '@angular/material/progress-bar';

@Component({
  selector: 'app-quiz-timer',
  imports: [CommonModule, MatProgressBarModule],
  templateUrl: './quiz-timer.html',
  styleUrl: './quiz-timer.css',
})
export class QuizTimerComponent {
  @Input() progress: number = 0;
}
