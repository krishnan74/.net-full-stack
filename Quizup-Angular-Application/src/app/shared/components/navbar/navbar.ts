import { Component } from '@angular/core';
import { selectUser } from '../../../store/auth/state/auth.selectors';
import { Store } from '@ngrx/store';
import { Router, RouterLink } from '@angular/router';
import * as AuthActions from '../../../store/auth/state/auth.actions';
import { User } from '../../../store/auth/auth.model';
import { CommonModule } from '@angular/common';
import { ThemeService } from '../../../core/services/theme.service';
import { ToastService } from '../../../core/services/toast.service';

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
  currentTheme$ = this.themeService.theme$;

  constructor(
    private store: Store,
    private router: Router,
    private themeService: ThemeService,
    private toastService: ToastService
  ) {
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

  onLogoClick() {
    this.router.navigate(['/']);
  }

  onLogout() {
    this.store.dispatch(AuthActions.logout());
    this.toastService.showInfo(
      'Logout',
      'You have been successfully logged out.'
    );
  }

  toggleTheme() {
    this.themeService.toggleTheme();
    const currentTheme = this.themeService.getCurrentTheme();
    const themeName = currentTheme === 'light' ? 'Light' : 'Dark';
    this.toastService.showInfo(
      'Theme Changed',
      `Switched to ${themeName} mode`
    );
  }
}
