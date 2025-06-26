import {
  ApplicationConfig,
  provideBrowserGlobalErrorListeners,
  provideZoneChangeDetection,
} from '@angular/core';
import { provideRouter } from '@angular/router';
import { API_BASE_URL } from './core/tokens/api-url.token';

import { routes } from './app.routes';
import { HTTP_INTERCEPTORS, provideHttpClient } from '@angular/common/http';
import { provideStore } from '@ngrx/store';
import { provideEffects } from '@ngrx/effects';
import { authReducer } from './store/auth/state/auth.reducer';
import { AuthEffects } from './store/auth/state/auth.effects';
import { AuthService } from './store/auth/auth.service';
import { TeacherService } from './features/teacher/services/teacher.service';
import { StudentService } from './features/student/services/student.service';
import { AuthInterceptor } from './core/interceptors/auth-interceptor';
import { QuizService } from './features/quiz/services/quiz.service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(),
    {
      provide: API_BASE_URL,
      useValue: 'http://localhost:5166/api/v1',
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
    provideStore({ auth: authReducer }),
    provideEffects([AuthEffects]),
    AuthService,
    TeacherService,
    StudentService,
    QuizService
  ],
};
