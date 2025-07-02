import { BehaviorSubject, map, Observable } from 'rxjs';
import { Inject, inject, Injectable } from '@angular/core';
import { API_BASE_URL } from '../../../core/tokens/api-url.token';
import { HttpClient } from '@angular/common/http';
import { ApiResponse } from '../../../shared/models/api-response';
import { User } from '../../../store/auth/auth.model';
import { SubjectModel } from '../models/subject';

@Injectable()
export class SubjectService {
  constructor(
    private http: HttpClient,
    @Inject(API_BASE_URL) private apiBaseUrl: string
  ) {}

  getSubjectById(id: number) {
    const subject = this.http.get<SubjectModel>(`${this.apiBaseUrl}/Subjects/${id}`);
    console.log('Subject fetched:', subject);
    return subject;
  }

  createSubject(
    subject: Omit<SubjectModel, 'id' | 'createdAt' | 'updatedAt'>
  ): Observable<ApiResponse<SubjectModel>> {
    try {
      console.log('Creating subject with data:', subject);
      const createdSubject = this.http.post<ApiResponse<SubjectModel>>(
        `${this.apiBaseUrl}/Subjects`,
        subject
      );

      console.log('Subject created:', createdSubject);
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

    console.log('All subjects fetched:', subjects);
    return subjects;
  }
}
