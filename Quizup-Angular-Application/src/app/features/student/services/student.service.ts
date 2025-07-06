import { BehaviorSubject, map, Observable } from 'rxjs';
import { StudentModel } from '../models/student';
import { Inject, inject, Injectable } from '@angular/core';
import { API_BASE_URL } from '../../../core/tokens/api-url.token';
import { HttpClient } from '@angular/common/http';
import { ApiResponse } from '../../../shared/models/api-response';
import { AnswerModel } from '../../quiz/models/answer.model';
import { QuizSubmissionModel } from '../../../shared/models/quiz-submission.model';

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

  attemptQuiz(
    studentId: number,
    quizId: number
  ): Observable<ApiResponse<QuizSubmissionModel>> {
    const attempt = this.http
      .post<ApiResponse<QuizSubmissionModel>>(
        `${this.apiBaseUrl}/Students/${studentId}/quizzes/${quizId}/start`,
        {}
      )
      .pipe(
        map((response) => {
          console.log('Quiz attempt response:', response);
          return response;
        })
      );
    return attempt;
  }

  submitQuiz(
    studentId: number,
    submissionId: number,
    answers: Omit<AnswerModel, 'id' | 'quizSubmissionId'>[]
  ): Observable<ApiResponse<QuizSubmissionModel>> {
    const submission = this.http
      .post<ApiResponse<QuizSubmissionModel>>(
        `${this.apiBaseUrl}/Students/${studentId}/submissions/${submissionId}/submit`,
        { answers }
      )
      .pipe(
        map((response) => {
          console.log('Quiz submission response:', response);
          return response;
        })
      );
    return submission;
  }

  checkIfQuizAttemptExists(
    studentId: number,
    quizId: number
  ): Observable<number | null> {
    return this.http
      .get<ApiResponse<QuizSubmissionModel[]>>(
        `${this.apiBaseUrl}/Students/${studentId}/submissions`
      )
      .pipe(
        map((response) => {
          const submissions = response.data;
          console.log('Submissions fetched:', submissions);
          const submissionId =
            submissions.find((submission) => submission.quizId === quizId)
              ?.id || null;
          console.log(
            `Quiz attempt exists for student ${studentId} and quiz ${quizId}:`,
            submissionId
          );
          return submissionId;
        })
      );
  }

}
