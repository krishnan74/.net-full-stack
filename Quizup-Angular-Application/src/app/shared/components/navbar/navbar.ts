import { Component } from '@angular/core';
import { selectUser } from '../../../store/auth/state/auth.selectors';
import { Store } from '@ngrx/store';
import { RouterLink } from '@angular/router';
import * as AuthActions from '../../../store/auth/state/auth.actions';
import { User } from '../../../store/auth/auth.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink, CommonModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class Navbar {
  user$ = this.store.select(selectUser);
  user: User | null = null;
  isScrolled: boolean = false;
  lastScrollTop: number = 0;

  constructor(private store: Store) {
    window.addEventListener('scroll', this.onScroll.bind(this));
  }

  onScroll() {
    const st = window.scrollY || document.documentElement.scrollTop;
    if (st > this.lastScrollTop && st > 100) {
      this.isScrolled = true;
    } else if (st <= 0) {
      this.isScrolled = false;
    }
    this.lastScrollTop = st <= 0 ? 0 : st;
  }

  ngOnInit() {
    this.user$.subscribe((user) => {
      if (user) {
        this.user = user;
      } else {
        this.user = null;
      }
    });
  }

  onLogout() {
    this.store.dispatch(AuthActions.logout());
  }
}
