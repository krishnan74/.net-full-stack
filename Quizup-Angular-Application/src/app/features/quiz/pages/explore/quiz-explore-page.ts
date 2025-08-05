import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { QuizList } from '../../components/quiz-list/quiz-list';
import { RouterLink } from '@angular/router';
import { Store } from '@ngrx/store';
import { selectUser } from '../../../../store/auth/state/auth.selectors';
import { User } from '../../../../store/auth/auth.model';

@Component({
  selector: 'app-quiz-explore-page',
  templateUrl: './quiz-explore-page.component.html',
  styleUrls: ['./quiz-explore-page.component.css'],
  imports: [CommonModule, QuizList, RouterLink],
})
export class QuizExplorePage {
  user$ = this.store.select(selectUser);
  user: User | null = null;

  constructor(private store: Store) {
    this.user$.subscribe((user) => {
      this.user = user;
    });
  }
}
