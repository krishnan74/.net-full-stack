import {
  ApplicationConfig,
  provideBrowserGlobalErrorListeners,
  provideZoneChangeDetection,
} from '@angular/core';
import { provideRouter } from '@angular/router';
import { API_BASE_URL } from './core/tokens/api-url.token';

import { routes } from './app.routes';
import { provideHttpClient } from '@angular/common/http';
import { TeacherService } from './features/teacher/services/teacher.service';

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
    TeacherService,
  ],
};
