import { BehaviorSubject, map, Observable } from 'rxjs';
import { Teacher } from '../models/teacher';
import { Inject, inject, Injectable } from '@angular/core';
import { API_BASE_URL } from '../../../core/tokens/api-url.token';
import { HttpClient } from '@angular/common/http';
import { ApiResponse } from '../../../shared/models/api-response';
import { User } from '../../../store/auth/auth.model';

@Injectable()
export class TeacherService {
  constructor(
    private http: HttpClient,
    @Inject(API_BASE_URL) private apiBaseUrl: string
  ) {}
  getTeacherById(id: number) {
    const teacher = this.http.get<Teacher>(`${this.apiBaseUrl}/Teachers/${id}`);
    console.log('Teacher fetched:', teacher);
    return teacher;
  }

  createTeacher(
    teacher: Omit<Teacher, 'id' | 'createdAt' | 'quizzes'> & {
      password: string;
      role: string;
    }
  ): Observable<ApiResponse<Teacher>> {
    try {
      console.log('Creating teacher with data:', teacher);
      const createdTeacher = this.http.post<ApiResponse<Teacher>>(
        `${this.apiBaseUrl}/Teachers`,
        teacher
      );

      console.log('Teacher created:', createdTeacher);
      return createdTeacher;
    } catch (error) {
      console.error('Error creating teacher:', error);
      throw error;
    }
  }

  getAllTeachers(): Observable<ApiResponse<Teacher[]>> {
    const teachers = this.http.get<ApiResponse<Teacher[]>>(
      `${this.apiBaseUrl}/Teachers`
    );

    console.log('All teachers fetched:', teachers);
    return teachers;
  }
}
