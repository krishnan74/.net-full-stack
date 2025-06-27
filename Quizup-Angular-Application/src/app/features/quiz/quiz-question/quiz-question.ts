import { Component, Input, OnInit } from '@angular/core';
import { QuestionModel } from '../models/question.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-quiz-question',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './quiz-question.html',
  styleUrl: './quiz-question.css',
})
export class QuizQuestion implements OnInit {
  @Input() question: QuestionModel | null = null;

  ngOnChanges() {
    console.log('QuizQuestion input updated:', this.question);
  }

  ngOnInit() {
    // Optional: use only for non-@Input setup
  }
}
