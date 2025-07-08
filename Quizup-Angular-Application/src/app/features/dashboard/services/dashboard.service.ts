import { BehaviorSubject, map, Observable } from 'rxjs';
import { Inject, inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL } from '../../../core/tokens/api-url.token';
import { TeacherSummary } from '../../teacher/models/teacher-summary.model';
import { StudentSummary } from '../../student/models/student-summary.model';
import { ApiResponse } from '../../../shared/models/api-response';

@Injectable()
export class DashboardService {
  constructor(
    private http: HttpClient,
    @Inject(API_BASE_URL) private apiBaseUrl: string
  ) {}

  getStudentSummary(userId: number): Observable<StudentSummary> {
    console.log(`Fetching student summary for userId: ${userId}`);
    const summary = this.http
      .get<ApiResponse<StudentSummary>>(
        `${this.apiBaseUrl}/Students/${userId}/summary`
      )
      .pipe(
        map((res) => {
          console.log('API Response for Student Summary:', res);
          return res.data;
        })
      );
    return summary;
  }

  getTeacherSummary(userId: number): Observable<TeacherSummary> {
    const summary = this.http
      .get<ApiResponse<TeacherSummary>>(
        `${this.apiBaseUrl}/Teachers/${userId}/summary`
      )
      .pipe(map((res) => res.data));
    return summary;
  }
}
