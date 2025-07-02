import { BehaviorSubject, Observable } from 'rxjs';
import { Inject, inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_BASE_URL } from '../../core/tokens/api-url.token';
import { TeacherSummary } from '../teacher/models/teacher-summary.model';
import { StudentSummary } from '../student/models/student-summary.model';
import { ApiResponse } from '../../shared/models/api-response';

@Injectable()
export class DashboardService {
  constructor(
    private http: HttpClient,
    @Inject(API_BASE_URL) private apiBaseUrl: string
  ) {}

  getStudentSummary(
    studentId: number
  ): Observable<ApiResponse<StudentSummary>> {
    return this.http.get<ApiResponse<StudentSummary>>(
      `${this.apiBaseUrl}/Students/${studentId}/summary`
    );
  }

  getTeacherSummary(
    teacherId: number
  ): Observable<ApiResponse<TeacherSummary>> {
    return this.http.get<ApiResponse<TeacherSummary>>(
      `${this.apiBaseUrl}/Teachers/${teacherId}/summary`
    );
  }
}
