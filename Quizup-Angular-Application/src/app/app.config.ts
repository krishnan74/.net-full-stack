import {
  ApplicationConfig,
  provideBrowserGlobalErrorListeners,
  provideZoneChangeDetection,
} from '@angular/core';
import { provideRouter } from '@angular/router';
import { API_BASE_URL } from './core/tokens/api-url.token';
import { provideCharts, withDefaultRegisterables } from 'ng2-charts';
import { routes } from './app.routes';
import {
  HTTP_INTERCEPTORS,
  provideHttpClient,
  withInterceptorsFromDi,
} from '@angular/common/http';
import { provideStore } from '@ngrx/store';
import { provideEffects } from '@ngrx/effects';
import { authReducer } from './store/auth/state/auth.reducer';
import { AuthEffects } from './store/auth/state/auth.effects';
import { AuthService } from './store/auth/auth.service';
import { TeacherService } from './features/teacher/services/teacher.service';
import { StudentService } from './features/student/services/student.service';
import { SubjectService } from './features/subject/services/subject.service';
import { AuthInterceptor } from './core/interceptors/auth-interceptor';
import { QuizService } from './features/quiz/services/quiz.service';
import { localStorageMetaReducer } from './store/auth/state/meta.reducer';
import { ClassService } from './features/class/services/class.service';
import { DashboardService } from './features/dashboard/dashboard.service';
import { SignalRService } from './features/notification/services/signalr.service';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { providePrimeNG } from 'primeng/config';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideAnimationsAsync(),

    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withInterceptorsFromDi()),
    {
      provide: API_BASE_URL,
      useValue: 'http://localhost:5166/api/v1',
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
    provideStore(
      { auth: authReducer },
      { metaReducers: [localStorageMetaReducer] }
    ),
    provideEffects([AuthEffects]),
    provideCharts(withDefaultRegisterables()),
    AuthService,
    TeacherService,
    StudentService,
    QuizService,
    SubjectService,
    ClassService,
    DashboardService,
    SignalRService,
  ],
};