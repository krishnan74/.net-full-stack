import { Component, ChangeDetectionStrategy } from '@angular/core';
import { Store } from '@ngrx/store';
import { selectUser } from '../../store/auth/state/auth.selectors';
import { DashboardService } from './services/dashboard.service';
import { Observable, of } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { StudentSummary } from '../student/models/student-summary.model';
import { TeacherSummary } from '../teacher/models/teacher-summary.model';
import { ChartConfiguration } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

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

  viewSubmissionDetails(quizId: number, submissionId: number) {
    this.router.navigate(['/quiz/', quizId, 'submission', submissionId]);
  }

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
    private dashboardService: DashboardService,
    private router: Router
  ) {
    // Handle Student Summary
    this.studentSummary$.subscribe((summary) => {
      if (!summary) return;

      // Pie chart
      if (summary.quizzesByStatus) {
        const labels = Object.keys(summary.quizzesByStatus);
        const data = Object.values(summary.quizzesByStatus) as number[];
        this.pieCharData = {
          labels,
          datasets: [
            {
              data,
              backgroundColor: ['#4caf50', '#ff9800', '#03a9f4'],
            },
          ],
        };
      }

      // Line chart
      if (summary.performanceTrend) {
        const labels = summary.performanceTrend.map((p: any) =>
          new Date(p.month).toLocaleString('default', {
            month: 'short',
            year: 'numeric',
          })
        );

        const avgScoreData = summary.performanceTrend.map(
          (p: any) => p.avg_score
        );

        this.lineChartData = {
          labels,
          datasets: [
            {
              label: 'Average Score',
              data: avgScoreData,
              borderColor: '#3f51b5',
              backgroundColor: 'rgba(63,81,181,0.3)',
              fill: true,
              tension: 0.3,
            },
          ],
        };
      }
    });

    // Handle Teacher Summary
    this.teacherSummary$.subscribe((summary) => {
      if (!summary) return;

      // Pie chart for quiz status
      if (summary.quizzesByStatus) {
        const labels = Object.keys(summary.quizzesByStatus);
        const data = Object.values(summary.quizzesByStatus) as number[];
        this.pieCharData = {
          labels,
          datasets: [
            {
              data,
              backgroundColor: ['#4caf50', '#f44336'],
            },
          ],
        };
      }

      // Line chart for performance trend
      if (summary.quizPerformanceTrend) {
        const labels = summary.quizPerformanceTrend.map((p: any) =>
          new Date(p.month).toLocaleString('default', {
            month: 'short',
            year: 'numeric',
          })
        );

        const avgScoreData = summary.quizPerformanceTrend.map(
          (p: any) => p.avg_score
        );

        const completionRateData = summary.quizPerformanceTrend.map(
          (p: any) => p.completion_rate
        );

        this.lineChartData = {
          labels,
          datasets: [
            {
              label: 'Average Score',
              data: avgScoreData,
              borderColor: '#3f51b5',
              backgroundColor: 'rgba(63,81,181,0.2)',
              fill: true,
              tension: 0.3,
            },
            {
              label: 'Completion Rate (%)',
              data: completionRateData,
              borderColor: '#ff9800',
              backgroundColor: 'rgba(255,152,0,0.2)',
              fill: true,
              tension: 0.3,
            },
          ],
        };
      }
    });
  }
}
