import { Component } from '@angular/core';
import { selectUser } from '../../../store/auth/state/auth.selectors';
import { Store } from '@ngrx/store';
import { RouterLink } from '@angular/router';
import * as AuthActions from '../../../store/auth/state/auth.actions';
import { User } from '../../../store/auth/auth.model';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class Navbar {
  user$ = this.store.select(selectUser);
  user: User | null = null;
  constructor(private store: Store) {}

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
