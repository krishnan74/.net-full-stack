import { BehaviorSubject, Observable } from 'rxjs';
import { Teacher } from '../models/teacher';
import { Inject, inject, Injectable } from '@angular/core';
import { API_BASE_URL } from '../../../core/tokens/api-url.token';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class TeacherService {

  private teachersSubject = new BehaviorSubject<Teacher[]>([]);
  teachers$: Observable<Teacher[]> = this.teachersSubject.asObservable();

  constructor(
    private http: HttpClient,
        @Inject(API_BASE_URL) private apiBaseUrl: string
  ){

  }
  getTeacherById(id: number) {
    const teacher = this.http.get<Teacher>(`${this.apiBaseUrl}/Teachers/${id}`);
    console.log('Teacher fetched:', teacher);
    return teacher;
  }

  createTeacher(
    teacher: Omit<Teacher, 'id' | 'createdAt' | 'quizzes'> & {
      password: string;
    }
  ): Observable<Teacher> {
    const createdTeacher = this.http.post<Teacher>(
      `${this.apiBaseUrl}/Teachers`,
      teacher
    );
    console.log('Teacher created:', createdTeacher);
    return createdTeacher;
  }

  getAllTeachers(): Observable<Teacher[]> {
    const teachers = this.http.get<Teacher[]>(`${this.apiBaseUrl}/Teachers`);
    console.log('All teachers fetched:', teachers);
    return teachers;
  }
}
