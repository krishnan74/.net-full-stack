import { BehaviorSubject, map, Observable } from 'rxjs';
import { QuizCreateModel, QuizModel } from '../models/quiz.model';
import { Inject, inject, Injectable } from '@angular/core';
import { API_BASE_URL } from '../../../core/tokens/api-url.token';
import { HttpClient } from '@angular/common/http';
import { ApiResponse } from '../../../shared/models/api-response';

@Injectable()
export class QuizService {
  constructor(
    private http: HttpClient,

    @Inject(API_BASE_URL) private apiBaseUrl: string
  ) {}

  private quizzesSubject = new BehaviorSubject<QuizModel[]>([]);
  quizzes$: Observable<QuizModel[]> = this.quizzesSubject.asObservable();

  getQuizById(id: number): Observable<ApiResponse<QuizModel>> {
    const quiz = this.http.get<ApiResponse<QuizModel>>(
      `${this.apiBaseUrl}/Quizzes/${id}`
    );
    return quiz;
  }

  createQuiz(quiz: QuizCreateModel): Observable<ApiResponse<QuizModel>> {
    const createdQuiz = this.http.post<ApiResponse<QuizModel>>(
      `${this.apiBaseUrl}/Quizzes`,
      quiz
    );
    console.log('Quiz created:', createdQuiz);
    return createdQuiz;
  }

  searchQuizzes(
    searchTerm: string,
    createdAtMin?: Date,
    createdAtMax?: Date,
    dueDateMin?: Date,
    dueDateMax?: Date,
    isActive?: boolean
  ): Observable<QuizModel[]> {
    const quizzes = this.http.get<QuizModel[]>(`
        
        ${
          this.apiBaseUrl
        }/Quizzes/search?Title=${searchTerm}&Description=${searchTerm}&TeacherName=${searchTerm}&CreatedAtMin=${
      createdAtMin || ''
    }&CreatedAtMax=${createdAtMax || ''}&DueDateMin=${
      dueDateMin || ''
    }&DueDateMax=${dueDateMax || ''}&IsActive=${
      isActive !== undefined ? isActive : ''
    }`);

    console.log('Quizzes fetched:', quizzes);
    return quizzes;
  }

  deleteQuiz(id: number): Observable<ApiResponse<QuizModel>> {
    console.log('Deleting quiz with ID:', id);
    return this.http
      .delete<ApiResponse<QuizModel>>(`${this.apiBaseUrl}/Quizzes/${id}`)
      .pipe(
        map((response) => {
          console.log('Quiz deleted successfully:', response);
          return response;
        })
      );
  }
}
