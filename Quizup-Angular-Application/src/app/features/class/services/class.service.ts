import { map, Observable } from 'rxjs';
import { Inject, inject, Injectable } from '@angular/core';
import { API_BASE_URL } from '../../../core/tokens/api-url.token';
import { HttpClient } from '@angular/common/http';
import { ApiResponse } from '../../../shared/models/api-response';
import { User } from '../../../store/auth/auth.model';
import { ClassModel } from '../models/class.model';
import { SubjectModel } from '../../subject/models/subject.model';

@Injectable()
export class ClassService {
  constructor(
    private http: HttpClient,
    @Inject(API_BASE_URL) private apiBaseUrl: string
  ) {}

  createClass(
    classe: Omit<ClassModel, 'id' | 'createdAt' | 'updatedAt' | 'classSubjects'>
  ): Observable<ApiResponse<ClassModel>> {
    try {
      const createdClass = this.http.post<ApiResponse<ClassModel>>(
        `${this.apiBaseUrl}/Classes`,
        classe
      );

      return createdClass;
    } catch (error) {
      console.error('Error creating classe:', error);
      throw error;
    }
  }

  getClassById(id: number) {
    const classe = this.http.get<ClassModel>(
      `${this.apiBaseUrl}/Classes/${id}`
    );
    return classe;
  }

  getAllClasses(): Observable<ApiResponse<ClassModel[]>> {
    const classes = this.http.get<ApiResponse<ClassModel[]>>(
      `${this.apiBaseUrl}/Classes`
    );

    return classes;
  }

  updateClass(data: {
    id: number;
    className: string;
    addSubjectIds?: number[];
    removeSubjectIds?: number[];
  }): Observable<ApiResponse<ClassModel>> {
    return this.http
      .put<ApiResponse<ClassModel>>(`${this.apiBaseUrl}/Classes/${data.id}`, {
        className: data.className,
        addSubjectIds: data.addSubjectIds,
        removeSubjectIds: data.removeSubjectIds,
      })
      .pipe(
        map((response) => {
          return response;
        })
      );
  }

  deleteClass(id: number): Observable<ApiResponse<ClassModel>> {
    return this.http
      .delete<ApiResponse<ClassModel>>(`${this.apiBaseUrl}/Classes/${id}`)
      .pipe(
        map((response) => {
          return response;
        })
      );
  }

  getSubjectsByClassId(
    classId: number
  ): Observable<ApiResponse<SubjectModel[]>> {
    return this.http
      .get<ApiResponse<SubjectModel[]>>(
        `${this.apiBaseUrl}/Classes/${classId}/subjects`
      )
      .pipe(
        map((response) => {
          return response;
        })
      );
  }
}
