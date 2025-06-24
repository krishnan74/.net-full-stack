// src/app/store/auth/auth.service.ts

import { Inject, inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthPayload, User } from '../auth/auth.model';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../core/tokens/api-url.token';
import { ApiResponse } from '../../shared/models/api-response';

@Injectable()
export class AuthService {

  constructor(private http: HttpClient, 

    @Inject(API_BASE_URL) private apiBaseUrl: string

  ) {}

  login(payload: AuthPayload): Observable<ApiResponse<User>> {
    console.log(payload);
    return this.http.post<ApiResponse<User>>(`${this.apiBaseUrl}/Auth/login`, payload);
  }
}
