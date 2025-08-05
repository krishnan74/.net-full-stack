import { BehaviorSubject, map, Observable } from 'rxjs';
import { QuizCreateModel, QuizModel } from '../models/quiz.model';
import { Inject, inject, Injectable } from '@angular/core';
import { API_BASE_URL } from '../../../core/tokens/api-url.token';
import { HttpClient } from '@angular/common/http';
import { ApiResponse } from '../../../shared/models/api-response';
import { QuizSubmissionModel } from '../../../shared/models/quiz-submission.model';

@Injectable()
export class QuizService {
  constructor(
    private http: HttpClient,

    @Inject(API_BASE_URL) private apiBaseUrl: string
  ) {}

  private quizzesSubject = new BehaviorSubject<QuizModel[]>([]);
  quizzes$: Observable<QuizModel[]> = this.quizzesSubject.asObservable();

  getQuizById(id: number): Observable<ApiResponse<QuizModel>> {
    const quiz = this.http
      .get<ApiResponse<QuizModel>>(`${this.apiBaseUrl}/Quizzes/${id}`)
      .pipe(
        map((response) => {
          return response;
        })
      );
    return quiz;
  }

  createQuiz(quiz: QuizCreateModel): Observable<ApiResponse<QuizModel>> {
    const createdQuiz = this.http.post<ApiResponse<QuizModel>>(
      `${this.apiBaseUrl}/Quizzes`,
      quiz
    );
    return createdQuiz;
  }

  searchQuizzes(
    searchTerm: string,
    createdAtMin?: Date,
    createdAtMax?: Date,
    dueDateMin?: Date,
    dueDateMax?: Date,
    isActive?: boolean,
    subjectId?: number,
    classId?: number,
    userRole?: string,
    searchId?: number
  ): Observable<QuizModel[]> {
    console.log('Searching quizzes with parameters:', {
      searchTerm,
      createdAtMin,
      createdAtMax,
      dueDateMin,
      dueDateMax,
      isActive,
      subjectId,
      classId,
      userRole,
      searchId,
    });

    // Build query params, only include IsActive if not undefined/null
    let params = [
      `Title=${searchTerm}`,
      `Description=${searchTerm}`,
      `TeacherName=${searchTerm}`,
      `CreatedAtMin=${createdAtMin || ''}`,
      `CreatedAtMax=${createdAtMax || ''}`,
      `DueDateMin=${dueDateMin || ''}`,
      `DueDateMax=${dueDateMax || ''}`,
      userRole ? `Role=${userRole}` : '',
      searchId ? `SearchId=${searchId}` : '',
      subjectId ? `SubjectId=${subjectId}` : '',
      classId ? `ClassId=${classId}` : '',
      `Tags=${searchTerm || ''}`,
    ];
    if (isActive !== undefined && isActive !== null) {
      params.push(`IsActive=${isActive}`);
    }
    // Remove empty params
    params = params.filter((p) => p !== '');
    const queryString = params.join('&');
    const quizzes = this.http.get<QuizModel[]>(
      `${this.apiBaseUrl}/Quizzes/search?${queryString}`
    );
    return quizzes;
  }

  startQuiz(
    quizId: number,
    teacherId: number
  ): Observable<ApiResponse<QuizModel>> {
    return this.http
      .post<ApiResponse<QuizModel>>(
        `${this.apiBaseUrl}/Teachers/${teacherId}/quizzes/${quizId}/start`,
        {}
      )
      .pipe(
        map((response) => {
          return response;
        })
      );
  }

  endQuiz(
    quizId: number,
    teacherId: number
  ): Observable<ApiResponse<QuizModel>> {
    return this.http
      .post<ApiResponse<QuizModel>>(
        `${this.apiBaseUrl}/Teachers/${teacherId}/quizzes/${quizId}/end`,
        {}
      )
      .pipe(
        map((response) => {
          return response;
        })
      );
  }

  deleteQuiz(id: number): Observable<ApiResponse<QuizModel>> {
    return this.http
      .delete<ApiResponse<QuizModel>>(`${this.apiBaseUrl}/Quizzes/${id}`)
      .pipe(
        map((response) => {
          return response;
        })
      );
  }

  getQuizSubmissionById(
    id: number
  ): Observable<ApiResponse<QuizSubmissionModel>> {
    return this.http
      .get<ApiResponse<QuizSubmissionModel>>(
        `${this.apiBaseUrl}/Quizzes/submissions/${id}`
      )
      .pipe(
        map((response) => {
          return response;
        })
      );
  }
}
