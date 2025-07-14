import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { QuizList } from '../../components/quiz-list/quiz-list';

@Component({
  selector: 'app-quiz-explore-page',
  templateUrl: './quiz-explore-page.component.html',
  styleUrls: ['./quiz-explore-page.component.css'],
  imports: [CommonModule, QuizList],
})
export class QuizExplorePage {}
