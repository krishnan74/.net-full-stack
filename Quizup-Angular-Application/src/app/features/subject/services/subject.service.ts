import { BehaviorSubject, map, Observable } from 'rxjs';
import { Inject, inject, Injectable } from '@angular/core';
import { API_BASE_URL } from '../../../core/tokens/api-url.token';
import { HttpClient } from '@angular/common/http';
import { ApiResponse } from '../../../shared/models/api-response';
import { User } from '../../../store/auth/auth.model';
import { SubjectModel } from '../models/subject.model';

@Injectable()
export class SubjectService {
  constructor(
    private http: HttpClient,
    @Inject(API_BASE_URL) private apiBaseUrl: string
  ) {}

  getSubjectById(id: number) {
    const subject = this.http.get<SubjectModel>(
      `${this.apiBaseUrl}/Subjects/${id}`
    );
    return subject;
  }

  createSubject(
    subject: Omit<SubjectModel, 'id' | 'createdAt' | 'updatedAt'>
  ): Observable<ApiResponse<SubjectModel>> {
    try {
      const createdSubject = this.http.post<ApiResponse<SubjectModel>>(
        `${this.apiBaseUrl}/Subjects`,
        subject
      );

      return createdSubject;
    } catch (error) {
      console.error('Error creating subject:', error);
      throw error;
    }
  }

  getAllSubjects(): Observable<ApiResponse<SubjectModel[]>> {
    const subjects = this.http.get<ApiResponse<SubjectModel[]>>(
      `${this.apiBaseUrl}/Subjects`
    );

    return subjects;
  }

  deleteSubject(id: number): Observable<ApiResponse<SubjectModel>> {
    return this.http
      .delete<ApiResponse<SubjectModel>>(`${this.apiBaseUrl}/Subjects/${id}`)
      .pipe(
        map((response) => {
          return response;
        })
      );
  }
}
