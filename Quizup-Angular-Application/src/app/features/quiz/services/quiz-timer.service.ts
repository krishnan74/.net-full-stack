import { Injectable } from '@angular/core';
import { Subject, Observable, interval, takeUntil, take, map } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class QuizTimerService {
  private duration = 5000; // total duration in ms
  private intervalMs = 100; // update every 100ms
  private timer$ = new Subject<void>();

  startTimer(): Observable<number> {
    const steps = this.duration / this.intervalMs;
    return interval(this.intervalMs).pipe(
      takeUntil(this.timer$),
      take(steps),
      map((i) => (i + 1) / steps)
    );
  }

  stopTimer() {
    this.timer$.next();
  }
}
