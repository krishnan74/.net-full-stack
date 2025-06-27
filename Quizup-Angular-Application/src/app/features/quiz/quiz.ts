import { Component, inject, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { QuestionModel } from './models/question.model';
import { QuizService } from './services/quiz.service';
import { QuizQuestionModel } from './models/quizQuestion.model';
import { QuizQuestion } from './quiz-question/quiz-question';
import { QuizTimerComponent } from './components/quiz-timer/quiz-timer';
import { QuizTimerService } from './services/quiz-timer.service';

@Component({
  selector: 'app-quiz',
  imports: [RouterLink, QuizQuestion, QuizTimerComponent],
  templateUrl: './quiz.html',
  styleUrl: './quiz.css',
})
export class Quiz implements OnInit {
  router = inject(ActivatedRoute);
  progress: number = 0;
  quizId: number = 0;
  questions: QuestionModel[] = [];
  currentQuestionIndex: number = 0;
  constructor(
    private quizService: QuizService,
    private quizTimerService: QuizTimerService
  ) {}

  ngOnInit(): void {
    this.quizId = Number(this.router.snapshot.params['id']);

    this.quizService.getQuizById(this.quizId).subscribe({
      next: (data) => {
        const quizResponse = data;
        console.log('Quiz fetched:', quizResponse.data);

        if (quizResponse.data?.quizQuestions) {
          this.questions = quizResponse.data.quizQuestions.map(
            (q: QuizQuestionModel) => q.question
          );
        } else {
          this.questions = [];
        }

        console.log('Questions loaded:', this.questions[0]);
      },
      error: (err) => {
        console.error('Error fetching quiz questions:', err);
      },
    });

    this.quizTimerService.startTimer().subscribe({
      next: (value) => {
        this.progress = value * 100;
        console.log('Progress:', this.progress);
      },
      complete: () => {
        console.log('Progress completed ', this.progress);
        this.nextQuestion();
      },
    });
  }

  prevQuestion(): void {
    if (this.currentQuestionIndex > 0) {
      this.currentQuestionIndex--;
    } else {
      console.log('No previous question available.');
    }
  }

  nextQuestion(): void {
    if (this.currentQuestionIndex < this.questions.length - 1) {
      this.currentQuestionIndex++;
      this.quizTimerService.startTimer().subscribe({
        next: (value) => {
          this.progress = value * 100;
          console.log('Progress:', this.progress);
        },
        complete: () => {
          console.log('Progress completed ', this.progress);
          this.nextQuestion();
        },
      });
    } else {
      console.log('No more questions available.');
    }
  }
}
