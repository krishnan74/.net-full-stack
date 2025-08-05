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
  @Output() next = new EventEmitter<void>();

  selectedOption: string | null = null;

  ngOnChanges() {}

  ngOnInit() {}

  @Input() isLastQuestion: boolean = false;

  selectOption(option: string) {
    if (this.question) {
      this.selectedOption = option;
      this.answerSelected.emit({
        questionId: this.question.id!,
        selectedAnswer: option,
      });
      if (!this.isLastQuestion) {
        // Only disable and auto-advance for non-last questions
        setTimeout(() => {
          this.next.emit();
          this.selectedOption = null; // reset for next question
        }, 250);
      }
    }
  }
}
