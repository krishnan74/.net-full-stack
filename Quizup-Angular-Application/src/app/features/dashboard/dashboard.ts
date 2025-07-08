import { Component, ChangeDetectionStrategy } from '@angular/core';
import { Store } from '@ngrx/store';
import { selectUser } from '../../store/auth/state/auth.selectors';
import { DashboardService } from './services/dashboard.service';
import { Observable, of } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { StudentSummary } from '../student/models/student-summary.model';
import { TeacherSummary } from '../teacher/models/teacher-summary.model';
import { Chart, registerables, ChartConfiguration } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.css'],
  imports: [BaseChartDirective, CommonModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardComponent {
  user$ = this.store.select(selectUser);

  pieCharData: ChartConfiguration<'pie'>['data'] = {
    labels: [],
    datasets: [],
  };

  lineChartData: ChartConfiguration<'line'>['data'] = {
    labels: [],
    datasets: [],
  };

  pieOptions: ChartConfiguration<'pie'>['options'] = {
    plugins: { legend: { position: 'bottom' } },
  };

  lineOptions: ChartConfiguration<'line'>['options'] = {
    responsive: true,
    scales: { x: {}, y: { beginAtZero: true, max: 100 } },
  };

  studentSummary$: Observable<StudentSummary | null> = this.user$.pipe(
    switchMap((user) =>
      user?.role === 'Student'
        ? this.dashboardService.getStudentSummary(user.userId)
        : of(null)
    )
  );

  teacherSummary$: Observable<TeacherSummary | null> = this.user$.pipe(
    switchMap((user) =>
      user?.role === 'Teacher'
        ? this.dashboardService.getTeacherSummary(user.userId)
        : of(null)
    )
  );

  constructor(
    private store: Store,
    private dashboardService: DashboardService
  ) {
    this.user$.subscribe((user) => {
      console.log('User Observable Emission:', user);
    });

    this.studentSummary$.subscribe((summary) => {
      console.log('Student Summary Observable Emission:', summary);
    });

    this.teacherSummary$.subscribe((summary) => {
      console.log('Teacher Summary Observable Emission:', summary);
    });
  }
}
