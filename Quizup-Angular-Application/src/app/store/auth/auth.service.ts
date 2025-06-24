// src/app/store/auth/auth.service.ts

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthPayload, User } from '../auth/auth.model';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../core/tokens/api-url.token';

@Injectable({ providedIn: 'root' })
export class AuthService {
  constructor(private http: HttpClient) {}

  login(payload: AuthPayload): Observable<User> {
    return this.http.post<User>(`${API_BASE_URL}/login`, payload);
  }
}
