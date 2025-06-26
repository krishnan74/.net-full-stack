import { Component, Input, OnInit } from '@angular/core';
import { Question } from '../models/question';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-quiz-question',
  imports: [CommonModule],
  templateUrl: './quiz-question.html',
  styleUrl: './quiz-question.css',
})
export class QuizQuestion implements OnInit {
  @Input() question: Question | null = new Question();

  ngOnInit() {
    console.log(
      'QuizQuestion component initialized with question:',
      this.question
    );
  }
}
