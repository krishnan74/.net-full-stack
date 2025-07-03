import { BehaviorSubject, map, Observable } from 'rxjs';
import { StudentModel } from '../models/student';
import { Inject, inject, Injectable } from '@angular/core';
import { API_BASE_URL } from '../../../core/tokens/api-url.token';
import { HttpClient } from '@angular/common/http';
import { ApiResponse } from '../../../shared/models/api-response';

@Injectable()
export class StudentService {
  private studentsSubject = new BehaviorSubject<StudentModel[]>([]);
  students$: Observable<StudentModel[]> = this.studentsSubject.asObservable();

  constructor(
    private http: HttpClient,
    @Inject(API_BASE_URL) private apiBaseUrl: string
  ) {}

  getStudentById(id: number) {
    const student = this.http.get<StudentModel>(
      `${this.apiBaseUrl}/Students/${id}`
    );
    console.log('Student fetched:', student);
    return student;
  }

  createStudent(
    student: Omit<StudentModel, 'id' | 'createdAt' | 'quizzes'> & {
      password: string;
    }
  ): Observable<StudentModel> {
    try {
      const createdStudent = this.http.post<StudentModel>(
        `${this.apiBaseUrl}/Students`,
        student
      );

      console.log('Student created:', createdStudent);
      return createdStudent;
    } catch (error) {
      console.error('Error creating student:', error);
      throw error;
    }
  }

  getAllStudents(): Observable<ApiResponse<StudentModel[]>> {
    const students = this.http.get<ApiResponse<StudentModel[]>>(
      `${this.apiBaseUrl}/Students`
    );

    console.log('All students fetched:', students);
    return students;
  }

  deleteStudent(id: number): Observable<ApiResponse<StudentModel>> {
    console.log('Deleting student with ID:', id);
    return this.http
      .delete<ApiResponse<StudentModel>>(`${this.apiBaseUrl}/Students/${id}`)
      .pipe(
        map((response) => {
          console.log('Student deleted successfully:', response);
          return response;
        })
      );
  }
}
