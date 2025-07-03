// src/app/store/auth/auth.service.ts

import { Inject, inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthPayload, User } from '../auth/auth.model';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../core/tokens/api-url.token';
import { ApiResponse } from '../../shared/models/api-response';
import { SignalRService } from '../../features/notification/services/signalr.service';
import { tap } from 'rxjs/operators';

@Injectable()
export class AuthService {
  constructor(
    private http: HttpClient,

    @Inject(API_BASE_URL) private apiBaseUrl: string,

    private signalRService: SignalRService
  ) {}

  login(payload: AuthPayload): Observable<ApiResponse<User>> {
    return this.http
      .post<ApiResponse<User>>(`${this.apiBaseUrl}/Auth/login`, payload)
      .pipe(
        tap((response) => {
          const user = response.data;
          if (user.role === 'Student' && user.classGroupName) {
            this.signalRService.joinClassGroup(user.classGroupName);
          }
        })
      );
  }
}
