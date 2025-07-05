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
  showAttempt: boolean = true;

  user$ = this.store.select(selectUser);
  user: User | null = null;

  constructor(
    private router: Router,
    private studentService: StudentService,
    private store: Store
  ) {
    this.user$.subscribe((user) => {
      this.user = user;
      console.log('User fetched:', this.user);
    });
  }

  viewResults() {
    this.router.navigate(['/quiz/', this.quiz?.id, 'submission', 0]);
  }

  startQuiz() {
    if (this.quiz) {
      this.studentService
        .checkIfQuizAttemptExists(this.user?.userId!, this.quiz.id)
        .subscribe(
          (response) => {
            if (response != null) {
              console.log('Quiz attempt already exists:', response);
              this.router.navigate([
                '/quiz/',
                this.quiz?.id,
                'attempt',
                response,
              ]);
            } else {
              this.studentService
                .attemptQuiz(this.user?.userId!, this.quiz?.id!)
                .subscribe({
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
            }
          },
          (error) => {
            console.error('Error checking quiz attempt:', error);
          }
        );
    } else {
      console.error('Quiz data is not available');
    }
  }
}
