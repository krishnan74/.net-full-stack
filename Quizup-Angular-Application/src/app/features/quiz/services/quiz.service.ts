import { BehaviorSubject, Observable } from 'rxjs';
import { QuizModel } from '../models/quiz.model';
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

  createQuiz(
    quiz: Omit<QuizModel, 'id' | 'createdAt' | 'createdBy' | 'isActive'>
  ): Observable<QuizModel> {
    const createdQuiz = this.http.post<QuizModel>(
      `${this.apiBaseUrl}/quizzes`,
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
        
        ${this.apiBaseUrl}/Quizzes/search?Title=${searchTerm}&Description=${searchTerm}&TeacherName=${searchTerm}&CreatedAtMin=${createdAtMin?.toISOString() || ''}&CreatedAtMax=${createdAtMax?.toISOString() || ''}&DueDateMin=${dueDateMin?.toISOString() || ''}&DueDateMax=${dueDateMax?.toISOString() || ''}&IsActive=${isActive !== undefined ? isActive : ''}`);

    return quizzes;
  }
}
