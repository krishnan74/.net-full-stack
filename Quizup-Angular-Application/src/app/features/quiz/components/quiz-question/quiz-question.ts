import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { QuestionModel } from '../../models/question.model';
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
  @Output() answerSelected = new EventEmitter<{
    questionId: number;
    selectedAnswer: string;
  }>();

  ngOnChanges() {}

  ngOnInit() {}

  selectOption(option: string) {
    if (this.question) {
      console.log(
        `Selected option: ${option} for question ID: ${this.question.id}`
      );
      this.answerSelected.emit({
        questionId: this.question.id!,
        selectedAnswer: option,
      });
    }
  }
}
