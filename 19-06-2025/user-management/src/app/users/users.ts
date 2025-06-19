import { Component, HostListener, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {
  debounceTime,
  distinctUntilChanged,
  Subject,
  switchMap,
  tap,
} from 'rxjs';
import { UserService } from '../features/users/users.service';
import { UserModel } from '../features/users/user.model';
import { UserCard } from '../user-card/user-card';

@Component({
  selector: 'app-users',
  imports: [UserCard, FormsModule],
  templateUrl: './users.html',
  styleUrl: './users.css',
})
export class Users implements OnInit {
  users: UserModel[] = [];
  searchString: string = '';

  searchSubject = new Subject<string>();

  loading: boolean = false;
  showBackToTop: boolean = false;

  limit = 10;
  skip = 0;
  total = 0;

  constructor(private userService: UserService) {}

  handleSearchUsers() {
    this.searchSubject.next(this.searchString);
  }

  ngOnInit(): void {
    this.searchSubject
      .pipe(
        debounceTime(500),
        distinctUntilChanged(),
        tap(() => (this.loading = true)),
        switchMap((query) =>
          this.userService.getUserSearchResult(query, this.limit, this.skip)
        ),
        tap(() => (this.loading = false))
      )
      .subscribe({
        next: (data: any) => {
          this.users = data.users as UserModel[];
          this.total = data.total;
          console.log(this.total);
        },
      });
  }

  @HostListener('window:scroll', [])
  onScroll(): void {
    const scrollPosition = window.innerHeight + window.scrollY;
    const threshold = document.body.offsetHeight - 100;

    if (
      scrollPosition >= threshold &&
      !this.loading &&
      this.users?.length < this.total
    ) {
      this.loadMore();
    }

    this.showBackToTop = window.scrollY > 300;
  }

  scrollToTop() {
    window.scrollTo({
      top: 0,
      behavior: 'smooth',
    });
  }

  loadMore() {
    if (this.loading || this.users.length >= this.total) {
      return;
    }
    this.loading = true;
    this.skip += this.limit;

    const currentScroll = window.scrollY;
    this.userService
      .getUserSearchResult(this.searchString, this.limit, this.skip)
      .subscribe({
        next: (data: any) => {
          this.users = [...this.users, ...data.users];
          this.loading = false;

          setTimeout(() => {
            window.scrollTo(0, currentScroll);
          }, 0);
        },
        error: () => {
          this.loading = false;
        },
      });
  }
}
