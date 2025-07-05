import { Component, inject, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { QuestionModel } from './models/question.model';
import { QuizService } from './services/quiz.service';
import { QuizQuestionModel } from './models/quizQuestion.model';
import { QuizQuestion } from './quiz-question/quiz-question';
import { QuizTimerComponent } from './components/quiz-timer/quiz-timer';
import { QuizTimerService } from './services/quiz-timer.service';
import { AnswerModel } from './models/answer.model';
import { selectUser } from '../../store/auth/state/auth.selectors';
import { Store } from '@ngrx/store';
import { StudentService } from '../student/services/student.service';
import { User } from '../../store/auth/auth.model';

@Component({
  selector: 'app-quiz',
  imports: [RouterLink, QuizQuestion, QuizTimerComponent],
  templateUrl: './quiz.html',
  styleUrl: './quiz.css',
})
export class Quiz implements OnInit {
  user$ = this.store.select(selectUser);
  user: User | null = null;

  router = inject(ActivatedRoute);
  progress: number = 0;
  quizId: number = 0;
  submissionId: number = 0;
  questions: QuestionModel[] = [];
  currentQuestionIndex: number = 0;
  quizAnswers: Omit<AnswerModel, 'id' | 'quizSubmissionId'>[] = [];
  constructor(
    private quizService: QuizService,
    private quizTimerService: QuizTimerService,
    private studentService: StudentService,
    private store: Store
  ) {
    this.user$.subscribe((user) => {
      this.user = user;
      console.log('User fetched:', this.user);
    });
  }

  ngOnInit(): void {
    this.quizId = Number(this.router.snapshot.params['quizId']);
    this.submissionId = Number(this.router.snapshot.params['submissionId']);

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
      },
      complete: () => {
        this.nextQuestion();
      },
    });
  }

  selectAnswer(questionId: number, selectedAnswer: string) {
    this.quizAnswers[this.currentQuestionIndex] = {
      questionId: questionId,
      selectedAnswer: selectedAnswer,
    };

    console.log('Selected answers', this.quizAnswers);
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
        },
        complete: () => {
          this.nextQuestion();
        },
      });
    } else {
      this.studentService
        .submitQuiz(this.user?.userId || 0, this.submissionId, this.quizAnswers)
        .subscribe({
          next: (response) => {
            alert(
              `Quiz submitted successfully! Your score is: ${response.data.score}`
            );
          },
          error: (error) => {
            console.error('Error submitting quiz:', error);
          },
        });
      console.log('No more questions available.');
    }
  }
}
