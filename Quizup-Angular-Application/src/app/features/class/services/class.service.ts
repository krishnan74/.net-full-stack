import { map, Observable } from 'rxjs';
import { Inject, inject, Injectable } from '@angular/core';
import { API_BASE_URL } from '../../../core/tokens/api-url.token';
import { HttpClient } from '@angular/common/http';
import { ApiResponse } from '../../../shared/models/api-response';
import { User } from '../../../store/auth/auth.model';
import { ClassModel } from '../models/class';

@Injectable()
export class ClassService {
  constructor(
    private http: HttpClient,
    @Inject(API_BASE_URL) private apiBaseUrl: string
  ) {}

  getClassById(id: number) {
    const classe = this.http.get<ClassModel>(
      `${this.apiBaseUrl}/Classes/${id}`
    );
    console.log('Class fetched:', classe);
    return classe;
  }

  createClass(
    classe: Omit<ClassModel, 'id' | 'createdAt' | 'updatedAt' | 'classSubjects'>
  ): Observable<ApiResponse<ClassModel>> {
    try {
      console.log('Creating classe with data:', classe);
      const createdClass = this.http.post<ApiResponse<ClassModel>>(
        `${this.apiBaseUrl}/Classes`,
        classe
      );

      console.log('Class created:', createdClass);
      return createdClass;
    } catch (error) {
      console.error('Error creating classe:', error);
      throw error;
    }
  }

  getAllClasses(): Observable<ApiResponse<ClassModel[]>> {
    const classes = this.http.get<ApiResponse<ClassModel[]>>(
      `${this.apiBaseUrl}/Classes`
    );

    console.log('All classes fetched:', classes);
    return classes;
  }

  deleteClass(id: number): Observable<ApiResponse<ClassModel>> {
    console.log('Deleting class with ID:', id);
    return this.http
      .delete<ApiResponse<ClassModel>>(`${this.apiBaseUrl}/Classes/${id}`)
      .pipe(
        map((response) => {
          console.log('Class deleted successfully:', response);
          return response;
        })
      );
  }
}
