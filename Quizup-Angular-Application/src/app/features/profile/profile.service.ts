import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { ApiResponse } from '../../shared/models/api-response';
import { SubjectModel } from '../subject/models/subject.model';
import { API_BASE_URL } from '../../core/tokens/api-url.token';
import { ClassModel } from '../class/models/class.model';
import { QuestionModel } from '../quiz/models/question.model';

@Injectable({ providedIn: 'root' })
export class ProfileService {
  constructor(
    private http: HttpClient,
    @Inject(API_BASE_URL) private apiBaseUrl: string
  ) {}

  updateUserDetails(details: any): Observable<any> {
    return this.http.put(`${this.apiBaseUrl}/api/user`, details);
  }

  changePassword(
    currentPassword: string,
    newPassword: string
  ): Observable<any> {
    return this.http.post(`${this.apiBaseUrl}/Auth/change-password`, {
      currentPassword,
      newPassword,
    });
  }

  getSubjectsByStudentId(studentId: number): Observable<SubjectModel[]> {
    return this.http
      .get<ApiResponse<SubjectModel[]>>(
        `${this.apiBaseUrl}/Students/${studentId}/subjects`
      )
      .pipe(
        map((response) => {
          return response.data;
        })
      );
  }

  getClassesByTeacherId(teacherId: number): Observable<ClassModel[]> {
    return this.http
      .get<ApiResponse<ClassModel[]>>(
        `${this.apiBaseUrl}/Teachers/${teacherId}/classes`
      )
      .pipe(
        map((response) => {
          return response.data;
        })
      );
  }

  getSubjectsByTeacherId(teacherId: number): Observable<SubjectModel[]> {
    return this.http
      .get<ApiResponse<SubjectModel[]>>(
        `${this.apiBaseUrl}/Teachers/${teacherId}/subjects`
      )
      .pipe(
        map((response) => {
          return response.data;
        })
      );
  }

  getQuestionsByTeacherId(teacherId: number): Observable<QuestionModel[]> {
    return this.http
      .get<ApiResponse<QuestionModel[]>>(
        `${this.apiBaseUrl}/Teachers/${teacherId}/questions`
      )
      .pipe(
        map((response) => {
          return response.data;
        })
      );
  }
}
