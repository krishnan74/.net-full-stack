import { Component, Input } from '@angular/core';
import { QuizModel } from '../../models/quiz.model';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { StudentService } from '../../../student/services/student.service';
import { selectUser } from '../../../../store/auth/state/auth.selectors';
import { User } from '../../../../store/auth/auth.model';
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-quiz-card',
  imports: [CommonModule],
  templateUrl: './quiz-card.html',
  styleUrl: './quiz-card.css',
})
export class QuizCard {
  @Input() quiz: QuizModel | null = new QuizModel();
  @Input() showAttempt: boolean = true;
  @Input() userId: number | null = null;

  constructor(private router: Router, private studentService: StudentService) {}

  viewResults() {
    if (this.showAttempt) {
      this.router.navigate(['/quiz/', this.quiz?.id, 'submissions']);
    } else {
      this.router.navigate(['/quiz/', this.quiz?.id]);
    }
  }

  startQuiz() {
    if (this.quiz) {
      this.studentService.attemptQuiz(this.userId!, this.quiz?.id!).subscribe({
        next: (response) => {
          console.log('Quiz attempt started:', response);
          this.router.navigate([
            '/quiz/',
            this.quiz?.id,
            'attempt',
            response.data.id,
          ]);
        },
        error: (error) => {
          console.error('Error starting quiz attempt:', error);
        },
      });
    } else {
      console.error('Quiz data is not available');
    }
  }
}
