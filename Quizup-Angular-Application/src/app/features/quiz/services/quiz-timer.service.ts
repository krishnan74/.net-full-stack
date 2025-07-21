import { Injectable } from '@angular/core';
import { Subject, Observable, interval, takeUntil, take, map } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class QuizTimerService {
  private duration = 5000;
  private timer$ = new Subject<void>();

  startTimer(): Observable<number> {
    return interval(5000).pipe(
      takeUntil(this.timer$),
      take(this.duration / 5000),
      map((i) => (i + 1) / (this.duration / 5000))
    );
  }

  stopTimer() {
    this.timer$.next();
  }
}
