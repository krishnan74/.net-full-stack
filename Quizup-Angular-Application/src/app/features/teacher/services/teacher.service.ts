import { BehaviorSubject, map, Observable } from 'rxjs';
import { TeacherModel, TeacherUpdateModel } from '../models/teacher.model';
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
    const teacher = this.http.get<TeacherModel>(
      `${this.apiBaseUrl}/Teachers/${id}`
    );
    console.log('Teacher fetched:', teacher);
    return teacher;
  }

  createTeacher(
    teacher: Omit<TeacherModel, 'id' | 'createdAt' | 'quizzes'> & {
      password: string;
      role: string;
    }
  ): Observable<ApiResponse<TeacherModel>> {
    try {
      console.log('Creating teacher with data:', teacher);
      const createdTeacher = this.http.post<ApiResponse<TeacherModel>>(
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

  getAllTeachers(): Observable<ApiResponse<TeacherModel[]>> {
    const teachers = this.http.get<ApiResponse<TeacherModel[]>>(
      `${this.apiBaseUrl}/Teachers`
    );

    console.log('All teachers fetched:', teachers);
    return teachers;
  }

  updateTeacher(
    teacher: TeacherUpdateModel
  ): Observable<ApiResponse<TeacherModel>> {
    console.log('Updating teacher:', teacher);
    return this.http
      .put<ApiResponse<TeacherModel>>(
        `${this.apiBaseUrl}/Teachers/${teacher.id}`,
        teacher
      )
      .pipe(
        map((response) => {
          console.log('Teacher updated successfully:', response);
          return response;
        })
      );
  }

  deleteTeacher(id: number): Observable<ApiResponse<TeacherModel>> {
    console.log('Deleting teacher with ID:', id);
    return this.http
      .delete<ApiResponse<TeacherModel>>(`${this.apiBaseUrl}/Teachers/${id}`)
      .pipe(
        map((response) => {
          console.log('Teacher deleted successfully:', response);
          return response;
        })
      );
  }
}
