import { Component, OnInit } from '@angular/core';
import { DashboardService } from './dashboard.service';
import { Store } from '@ngrx/store';
import { selectUser } from '../../store/auth/state/auth.selectors';
import { TeacherSummary } from '../teacher/models/teacher-summary.model';
import { StudentSummary } from '../student/models/student-summary.model';
import { User } from '../../store/auth/auth.model';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.css'],
})
export class DashboardComponent implements OnInit {
  user$ = this.store.select(selectUser);
  user: User | null = null;
  teacherSummary: TeacherSummary | null = null;
  studentSummary: StudentSummary | null = null;

  constructor(
    private dashboardService: DashboardService,
    private store: Store
  ) {}

  ngOnInit() {
    this.user$.subscribe((user) => {
      if (user) {
        this.user = user;
      } else {
        this.user = null;
      }
    });

    if (this.user?.role === 'student') {
      this.getStudentSummary();
    }
    if (this.user?.role === 'teacher') {
      this.getTeacherSummary();
    }
  }

  getStudentSummary() {
    this.dashboardService.getStudentSummary(this.user!.id).subscribe((data) => {
      this.studentSummary = data.data as StudentSummary;
    });
  }

  getTeacherSummary() {
    this.dashboardService.getTeacherSummary(this.user!.id).subscribe((data) => {
      this.teacherSummary = data.data as TeacherSummary;
    });
  }
}
