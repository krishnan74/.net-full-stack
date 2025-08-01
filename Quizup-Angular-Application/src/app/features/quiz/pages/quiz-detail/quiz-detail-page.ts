import { Component } from '@angular/core';
import { QuizModel } from '../../models/quiz.model';
import { QuizService } from '../../services/quiz.service';
import { QuestionCard } from '../../components/question-card/question-card';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from '../../../../store/auth/auth.model';
import { Store } from '@ngrx/store';
import { selectUser } from '../../../../store/auth/state/auth.selectors';

@Component({
  selector: 'app-quiz-detail-page',
  imports: [QuestionCard, CommonModule],
  templateUrl: './quiz-detail-page.html',
  styleUrl: './quiz-detail-page.css',
})
export class QuizDetailPage {
  quiz: QuizModel = new QuizModel();
  sortKey: string = '';
  sortDirection: 'asc' | 'desc' = 'asc';
  quizId: number = 0;

  user$ = this.store.select(selectUser);
  user: User | null = null;

  constructor(
    private quizService: QuizService,
    private router: Router,
    private route: ActivatedRoute,
    private store: Store
  ) {
    this.user$.subscribe((user) => {
      this.user = user;
    });
    this.quizId = Number(this.route.snapshot.params['quizId']);
    this.loadQuiz();
  }

  viewSubmission(submissionId: number) {
    console.log('Viewing submission:', submissionId);
    this.router.navigate(['/quiz', this.quiz.id, 'submission', submissionId]);
  }

  sortSubmissions(key: string) {
    if (this.sortKey === key) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortKey = key;
      this.sortDirection = 'asc';
    }
    if (!this.quiz.submissions) return;
    this.quiz.submissions = [...this.quiz.submissions].sort(
      (a: any, b: any) => {
        let aValue = a;
        let bValue = b;
        if (key === 'studentName') {
          aValue = (a.student?.firstName || '') + (a.student?.lastName || '');
          bValue = (b.student?.firstName || '') + (b.student?.lastName || '');
        } else {
          aValue = a[key];
          bValue = b[key];
        }
        if (aValue == null) return 1;
        if (bValue == null) return -1;
        if (aValue < bValue) return this.sortDirection === 'asc' ? -1 : 1;
        if (aValue > bValue) return this.sortDirection === 'asc' ? 1 : -1;
        return 0;
      }
    );
  }

  loadQuiz() {
    this.quizService.getQuizById(this.quizId).subscribe((quiz) => {
      this.quiz = quiz.data;
      console.log('Quiz loaded:', this.quiz);
    });
  }

  toggleQuizStatus() {
    if (this.quiz.isActive) {
      this.quizService.endQuiz(this.quizId, this.user?.userId || 0).subscribe({
        next: (response) => {
          console.log('Quiz ended successfully:', response);
          this.quiz.isActive = false;
        },
        error: (error) => {
          console.error('Error ending quiz:', error);
        }
      });
    } else {
      this.quizService.startQuiz(this.quizId, this.user?.userId || 0).subscribe({
        next: (response) => {
          console.log('Quiz started successfully:', response);
          this.quiz.isActive = true;
        },
        error: (error) => {
          console.error('Error starting quiz:', error);
        }
      });
    }

    this.loadQuiz();
  }
}
