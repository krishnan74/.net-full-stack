import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { QuizCreateComponent } from '../../components/quiz-creation/quiz-create.component';

@Component({
  selector: 'app-quiz-create-page',
  templateUrl: './quiz-create-page.component.html',
  styleUrls: ['./quiz-create-page.component.css'],
  imports: [CommonModule, QuizCreateComponent],
})
export class QuizCreatePage {}
