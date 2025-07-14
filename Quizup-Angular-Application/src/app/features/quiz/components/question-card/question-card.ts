import { Component, Input } from '@angular/core';
import { QuestionModel } from '../../models/question.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-question-card',
  imports: [CommonModule],
  templateUrl: './question-card.html',
  styleUrl: './question-card.css',
})
export class QuestionCard {
  @Input() question!: QuestionModel;
  @Input() index: number = 0;
}
