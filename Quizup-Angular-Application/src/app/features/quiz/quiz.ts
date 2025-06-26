import { Component, inject, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Question } from './models/question';
import { QuizService } from './services/quiz.service';
import { QuizQuestion } from './quiz-question/quiz-question';
import { ApiResponse } from '../../shared/models/api-response';

@Component({
  selector: 'app-quiz',
  imports: [RouterLink, QuizQuestion],
  templateUrl: './quiz.html',
  styleUrl: './quiz.css',
})
export class Quiz implements OnInit {
  router = inject(ActivatedRoute);

  quizId: number = 0;
  questions: Question[] = [];
  currentQuestionIndex: number = 0;
  constructor(private quizService: QuizService) {}

  ngOnInit(): void {
    this.quizId = Number(this.router.snapshot.params['id']);
    this.quizService.getQuizById(this.quizId).subscribe({
      next: (data) => {
        const quizResponse = data;
        console.log('Quiz fetched:', quizResponse.data);
        if (quizResponse.data && quizResponse.data.questions) {
          this.questions = quizResponse.data.questions;
        }
      },
      error: (err) => {
        console.error('Error fetching quiz questions:', err);
      },
    });
  }

  nextQuestion(): void {
    if (this.currentQuestionIndex < this.questions.length - 1) {
      this.currentQuestionIndex++;
      const nextQuestion = this.questions[this.currentQuestionIndex];
      console.log('Next question:', nextQuestion);
      // Logic to navigate to the next question can be added here
    } else {
      console.log('No more questions available.');
    }
  }
}
