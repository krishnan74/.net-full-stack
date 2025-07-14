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

    const quizzes = this.http.get<QuizModel[]>(`
        ${
          this.apiBaseUrl
        }/Quizzes/search?Title=${searchTerm}&Description=${searchTerm}&TeacherName=${searchTerm}&CreatedAtMin=${
      createdAtMin || ''
    }&CreatedAtMax=${createdAtMax || ''}&DueDateMin=${
      dueDateMin || ''
    }&DueDateMax=${dueDateMax || ''}&IsActive=${
      isActive !== undefined ? isActive : ''
    }&Role=${userRole || ''}&SearchId=${searchId || ''}&SubjectId=${
      subjectId || ''
    }&ClassId=${classId || ''}&Tags=${searchTerm || ''}`);

    return quizzes;
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

  getQuizSubmissionId(
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
