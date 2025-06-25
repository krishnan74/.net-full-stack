import { BehaviorSubject, Observable } from 'rxjs';
import { Student } from '../models/student';
import { Inject, inject, Injectable } from '@angular/core';
import { API_BASE_URL } from '../../../core/tokens/api-url.token';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class StudentService {
  private studentsSubject = new BehaviorSubject<Student[]>([]);
  students$: Observable<Student[]> = this.studentsSubject.asObservable();

  constructor(
    private http: HttpClient,
    @Inject(API_BASE_URL) private apiBaseUrl: string
  ) {}
  getStudentById(id: number) {
    const student = this.http.get<Student>(`${this.apiBaseUrl}/Students/${id}`);
    console.log('Student fetched:', student);
    return student;
  }

  createStudent(
    student: Omit<Student, 'id' | 'createdAt' | 'quizzes'> & {
      password: string;
    }
  ): Observable<Student> {
    try {
      const createdStudent = this.http.post<Student>(
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

  getAllStudents(): Observable<Student[]> {
    const students = this.http.get<Student[]>(`${this.apiBaseUrl}/Students`);

    console.log('All students fetched:', students);
    return students;
  }
}
