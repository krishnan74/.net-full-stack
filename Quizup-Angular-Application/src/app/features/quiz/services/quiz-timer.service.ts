import { Injectable } from '@angular/core';
import { Subject, Observable, interval, takeUntil, take, map } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class QuizTimerService {
  private duration = 10000;
  private timer$ = new Subject<void>();

  startTimer(): Observable<number> {
    return interval(100).pipe(
      takeUntil(this.timer$),
      take(this.duration / 100),
      map((i) => (i + 1) / (this.duration / 100))
    );
  }

  stopTimer() {
    this.timer$.next();
  }
}
