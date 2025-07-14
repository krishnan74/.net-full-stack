import { Component } from '@angular/core';
import { QuizSubmissionModel } from '../../../../shared/models/quiz-submission.model';
import { QuizService } from '../../services/quiz.service';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { QuizModel } from '../../models/quiz.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-quiz-submission-page',
  imports: [CommonModule, FormsModule],
  templateUrl: './quiz-submission-page.html',
  styleUrl: './quiz-submission-page.css',
})
export class QuizSubmissionPage {
  quiz: QuizModel = new QuizModel();
  quizSubmission: QuizSubmissionModel = new QuizSubmissionModel(
    0,
    0,
    null as any,
    null as any,
    new Date(),
    new Date(),
    [],
    '',
    0
  );
  quizId: number = 0;
  submissionId: number = 0;

  constructor(private quizService: QuizService, private route: ActivatedRoute) {
    this.submissionId = Number(this.route.snapshot.params['submissionId']);
    this.quizId = Number(this.route.snapshot.params['quizId']);
    console.log('Quiz ID:', this.quizId);
    console.log('Submission ID:', this.submissionId);
    this.loadQuizSubmission();
    this.loadQuiz();
  }

  loadQuizSubmission() {
    this.quizService
      .getQuizSubmissionId(this.submissionId)
      .subscribe((response) => {
        this.quizSubmission = response.data;
        console.log('Quiz submission loaded:', this.quizSubmission);
      });
  }

  loadQuiz() {
    this.quizService.getQuizById(this.quizId).subscribe((response) => {
      this.quiz = response.data;
      console.log('Quiz loaded:', this.quiz);
    });
  }
}
