import { BehaviorSubject, Observable } from 'rxjs';
import { Quiz } from '../models/quiz';
import { inject, Injectable } from '@angular/core';
import { API_BASE_URL } from '../../../core/tokens/api-url.token';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class QuizService {
  private http = inject(HttpClient);
  private apiBaseUrl = inject(API_BASE_URL);

  private quizzesSubject = new BehaviorSubject<Quiz[]>([]);
  quizzes$: Observable<Quiz[]> = this.quizzesSubject.asObservable();

  getQuizById(id: number) {
    const quiz = this.http.get<Quiz>(`${this.apiBaseUrl}/quizzes/${id}`);
    console.log('Quiz fetched:', quiz);
    return quiz;
  }

  createQuiz(
    quiz: Omit<Quiz, 'id' | 'createdAt' | 'createdBy' | 'isActive'>
  ): Observable<Quiz> {
    const createdQuiz = this.http.post<Quiz>(
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
  ): Observable<Quiz[]> {
    const quizzes = this.http.get<Quiz[]>(`
        
        ${this.apiBaseUrl}/Quizzes/search
        ?Title=${searchTerm}&Description=${searchTerm}&TeacherName=${searchTerm}
        &CreatedAtMin=${createdAtMin?.toISOString() || ''}&CreatedAtMax=${
      createdAtMax?.toISOString() || ''
    }
        &DueDateMin=${dueDateMin?.toISOString() || ''}&DueDateMax=${
      dueDateMax?.toISOString() || ''
    }
        &IsActive=${isActive !== undefined ? isActive : ''}`);

    return quizzes;
  }
}
