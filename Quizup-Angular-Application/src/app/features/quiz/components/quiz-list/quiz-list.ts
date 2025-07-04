import { Component, OnInit } from '@angular/core';
import { QuizService } from '../../services/quiz.service';
import {
  debounceTime,
  distinctUntilChanged,
  Subject,
  switchMap,
  tap,
} from 'rxjs';
import { QuizModel } from '../../models/quiz.model';
import { QuizCard } from '../quiz-card/quiz-card';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-quiz-list',
  imports: [QuizCard, CommonModule, FormsModule],
  templateUrl: './quiz-list.html',
  styleUrl: './quiz-list.css',
})
export class QuizList implements OnInit {
  quizzes: QuizModel[] = [];

  searchString: string = '';
  createdAtMin?: Date;
  createdAtMax?: Date;
  dueDateMin?: Date;
  dueDateMax?: Date;
  isActive?: boolean;

  searchSubject = new Subject<void>();
  loading: boolean = false;

  constructor(private quizService: QuizService) {}

  handleSearchQuizzes() {
    this.searchSubject.next();
  }

  ngOnInit(): void {
    this.searchSubject
      .pipe(
        debounceTime(500),
        distinctUntilChanged(),
        tap(() => (this.loading = true)),
        switchMap(() =>
          this.quizService.searchQuizzes(
            this.searchString,
            this.createdAtMin,
            this.createdAtMax,
            this.dueDateMin,
            this.dueDateMax,
            this.isActive
          )
        ),
        tap(() => (this.loading = false))
      )
      .subscribe({
        next: (data: any) => {
          console.log(data);
          this.quizzes = data as QuizModel[];
        },
      });
  }
}
