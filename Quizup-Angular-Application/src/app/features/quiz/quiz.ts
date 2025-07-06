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
import { Toast } from 'primeng/toast';
import { Ripple } from 'primeng/ripple';
import { MessageService } from 'primeng/api';
import { CommonModule } from '@angular/common';
import { QuizResultDialog } from './components/quiz-result-dialog/quiz-result-dialog';

@Component({
  selector: 'app-quiz',
  imports: [
    RouterLink,
    QuizQuestion,
    QuizTimerComponent,
    Toast,
    Ripple,
    CommonModule,
    QuizResultDialog,
  ],
  templateUrl: './quiz.html',
  styleUrl: './quiz.css',
  providers: [MessageService],
})
export class Quiz implements OnInit {
  user$ = this.store.select(selectUser);
  user: User | null = null;

  progress: number = 0;
  quizId: number = 0;
  submissionId: number = 0;
  questions: QuestionModel[] = [];
  currentQuestionIndex: number = 0;
  quizAnswers: Omit<AnswerModel, 'id' | 'quizSubmissionId'>[] = [];
  showResultDialog: boolean = false;
  quizScore: number = 0;
  reviewMode: boolean = false;
  submissionAnswers: any[] = [];
  constructor(
    private quizService: QuizService,
    private quizTimerService: QuizTimerService,
    private studentService: StudentService,
    private store: Store,
    private route: ActivatedRoute,
    private router: Router,
    private messageService: MessageService
  ) {
    this.user$.subscribe((user) => {
      this.user = user;
      console.log('User fetched:', this.user);
    });
  }

  ngOnInit(): void {
    this.quizId = Number(this.route.snapshot.params['quizId']);
    this.submissionId = Number(this.route.snapshot.params['submissionId']);

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
            this.quizScore = response.data.score;
            this.showResultDialog = true;
            this.submissionAnswers = response.data.answers || [];
            this.messageService.add({
              severity: 'success',
              summary: 'Quiz Submitted',
              detail: `Quiz submitted successfully! Your score is: ${response.data.score}`,
              life: 3000,
            });
          },
          error: (error) => {
            this.messageService.add({
              severity: 'error',
              summary: 'Submission Error',
              detail: 'Error submitting quiz. Please try again.',
              life: 3000,
            });
            console.error('Error submitting quiz:', error);
          },
        });
      console.log('No more questions available.');
    }
  }

  reviewAnswers() {
    this.showResultDialog = false;
    this.reviewMode = true;
  }

  getReviewStatus(
    questionId: number,
    option: string
  ): 'correct' | 'incorrect' | null {
    if (!this.reviewMode || !this.submissionAnswers.length) return null;
    const answer = this.submissionAnswers.find(
      (a) => a.questionId === questionId
    );
    if (!answer) return null;
    if (answer.selectedAnswer === option) {
      return 'correct';
    } else {
      return 'incorrect';
    }
  }

  goBackToQuizzes() {
    this.showResultDialog = false;
    this.router.navigate(['/quizzes']);
  }
}
