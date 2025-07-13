import { Component, OnInit } from '@angular/core';
import { QuizService } from '../../services/quiz.service';
import { debounceTime, Subject, switchMap, tap } from 'rxjs';
import { QuizModel } from '../../models/quiz.model';
import { QuizCard } from '../quiz-card/quiz-card';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { selectUser } from '../../../../store/auth/state/auth.selectors';
import { Store } from '@ngrx/store';
import { User } from '../../../../store/auth/auth.model';

@Component({
  selector: 'app-quiz-list',
  imports: [QuizCard, CommonModule, FormsModule],
  templateUrl: './quiz-list.html',
  styleUrl: './quiz-list.css',
})
export class QuizList implements OnInit {
  quizzes: QuizModel[] = [];
  user$ = this.store.select(selectUser);
  user: User | null = null;

  searchString: string = '';
  createdAtMin?: Date;
  createdAtMax?: Date;
  dueDateMin?: Date;
  dueDateMax?: Date;
  isActive?: boolean = false;

  searchSubject = new Subject<void>();
  loading: boolean = false;
  showFilters: boolean = true;
  filterToggle: boolean = false;
  lastScrollTop: number = 0;

  constructor(private quizService: QuizService, private store: Store) {
    window.addEventListener('scroll', this.onScroll.bind(this));
    this.user$.subscribe((user) => {
      this.user = user;
    });
  }

  handleSearchQuizzes() {
    this.searchSubject.next();
  }

  onScroll() {
    const st = window.scrollY || document.documentElement.scrollTop;
    if (st > this.lastScrollTop && st > 100 && !this.filterToggle) {
      this.showFilters = false;
    } else if (st <= 0) {
      this.showFilters = true;
      this.filterToggle = false;
    }
    this.lastScrollTop = st <= 0 ? 0 : st;
  }

  toggleFilters() {
    this.filterToggle = !this.filterToggle;
    this.showFilters = !this.showFilters;
  }

  ngOnInit(): void {
    this.searchSubject
      .pipe(
        debounceTime(500),
        tap(() => (this.loading = true)),
        switchMap(() =>
          this.quizService.searchQuizzes(
            this.searchString,
            this.createdAtMin,
            this.createdAtMax,
            this.dueDateMin,
            this.dueDateMax,
            this.isActive,
            this.user?.role,
            this.user?.userId
          )
        ),
        tap(() => (this.loading = false))
      )
      .subscribe({
        next: (data: any) => {
          console.log('Quizzes fetched:', data.data);
          this.quizzes = data.data as QuizModel[];
        },
      });
  }
}
