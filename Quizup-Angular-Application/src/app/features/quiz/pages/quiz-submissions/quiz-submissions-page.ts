import { Component } from '@angular/core';
import { QuizSubmissionModel } from '../../../../shared/models/quiz-submission.model';
import { ActivatedRoute, Router } from '@angular/router';
import { StudentService } from '../../../student/services/student.service';
import { User } from '../../../../store/auth/auth.model';
import { selectUser } from '../../../../store/auth/state/auth.selectors';
import { Store } from '@ngrx/store';
import { CommonModule } from '@angular/common';
import { QuizService } from '../../services/quiz.service';
import { QuizModel } from '../../models/quiz.model';
import { QuestionCard } from '../../components/question-card/question-card';

@Component({
  selector: 'app-quiz-submissions-page',
  imports: [CommonModule, QuestionCard],
  templateUrl: './quiz-submissions-page.html',
  styleUrl: './quiz-submissions-page.css',
})
export class QuizSubmissionsPage {
  quizSubmissions: QuizSubmissionModel[] = [];
  quizId: number = 0;
  user$ = this.store.select(selectUser);
  user: User | null = null;
  sortKey: string = '';
  sortDirection: 'asc' | 'desc' = 'asc';
  quiz: QuizModel = new QuizModel();

  constructor(
    private quizService: QuizService,
    private route: ActivatedRoute,
    private store: Store,
    private studentService: StudentService,
    private router: Router
  ) {
    this.user$.subscribe((user) => {
      this.user = user;
    });
    this.quizId = Number(this.route.snapshot.params['quizId']);
    this.loadQuiz();
    this.studentService
      .getQuizSubmissionsByStudentId(this.user?.userId!, this.quizId)
      .subscribe({
        next: (data) => {
          this.quizSubmissions = data.data;
        },
        error: (err) => {
          console.error('Error fetching quiz submissions:', err);
        },
      });
  }

  loadQuiz() {
    this.quizService.getQuizById(this.quizId).subscribe((quiz) => {
      this.quiz = quiz.data;
      console.log('Quiz loaded:', this.quiz);
    });
  }

  sortSubmissions(key: string) {
    if (this.sortKey === key) {
      this.sortDirection = this.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortKey = key;
      this.sortDirection = 'asc';
    }
    if (!this.quizSubmissions) return;
    this.quizSubmissions = [...this.quizSubmissions].sort((a: any, b: any) => {
      let aValue = a;
      let bValue = b;

      aValue = a[key];
      bValue = b[key];
      if (aValue == null) return 1;
      if (bValue == null) return -1;
      if (aValue < bValue) return this.sortDirection === 'asc' ? -1 : 1;
      if (aValue > bValue) return this.sortDirection === 'asc' ? 1 : -1;
      return 0;
    });
  }

  viewSubmissionDetails(submissionId: number) {
    this.router.navigate(['/quiz/', this.quizId, 'submission', submissionId]);
  }
}
