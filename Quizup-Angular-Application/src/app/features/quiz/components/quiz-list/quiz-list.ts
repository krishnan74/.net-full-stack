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
  searchSubject = new Subject<string>();
  loading: boolean = false;

  constructor(private quizService: QuizService) {}

  handleSearchQuizzes() {
    this.searchSubject.next(this.searchString);
  }

  ngOnInit(): void {
    this.searchSubject
      .pipe(
        debounceTime(500),
        distinctUntilChanged(),
        tap(() => (this.loading = true)),
        switchMap((query) =>
          this.quizService.searchQuizzes(
            query,
            new Date('2024-12-31'),
            new Date('2024-12-31'),
            new Date('2024-12-31'),
            new Date('2024-12-31'),
            false
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
