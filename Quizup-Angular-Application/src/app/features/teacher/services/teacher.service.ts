import { BehaviorSubject, map, Observable } from 'rxjs';
import { TeacherCreateModel, TeacherModel, TeacherUpdateModel } from '../models/teacher.model';
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
    return teacher;
  }

  createTeacher(
    teacher: TeacherCreateModel
  ): Observable<ApiResponse<TeacherModel>> {
    try {
      const createdTeacher = this.http.post<ApiResponse<TeacherModel>>(
        `${this.apiBaseUrl}/Teachers`,
        teacher
      );

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

    return teachers;
  }

  updateTeacher(
    teacher: TeacherUpdateModel
  ): Observable<ApiResponse<TeacherModel>> {
    return this.http
      .put<ApiResponse<TeacherModel>>(
        `${this.apiBaseUrl}/Teachers/${teacher.id}`,
        teacher
      )
      .pipe(
        map((response) => {
          return response;
        })
      );
  }

  deleteTeacher(id: number): Observable<ApiResponse<TeacherModel>> {
    return this.http
      .delete<ApiResponse<TeacherModel>>(`${this.apiBaseUrl}/Teachers/${id}`)
      .pipe(
        map((response) => {
          return response;
        })
      );
  }
}
