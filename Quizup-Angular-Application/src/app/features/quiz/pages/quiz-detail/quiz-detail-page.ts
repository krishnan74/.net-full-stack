import { Component } from '@angular/core';
import { QuizModel } from '../../models/quiz.model';
import { QuizService } from '../../services/quiz.service';
import { QuestionCard } from '../../components/question-card/question-card';
import { CommonModule } from '@angular/common';

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

  constructor(private quizService: QuizService) {
    this.loadQuiz();
  }

  viewSubmission(submissionId: number) {
    console.log('Viewing submission:', submissionId);
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
    this.quizService.getQuizById(1).subscribe((quiz) => {
      this.quiz = quiz.data;
      console.log('Quiz loaded:', this.quiz);
    });
  }
}
