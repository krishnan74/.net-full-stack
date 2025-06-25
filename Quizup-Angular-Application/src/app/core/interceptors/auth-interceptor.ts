// src/app/core/interceptors/auth.interceptor.ts

import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
} from '@angular/common/http';

import { Observable } from 'rxjs';
import { Store } from '@ngrx/store';
import { selectUser } from '../../store/auth/state/auth.selectors';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private token: string | null = null;

  constructor(private store: Store) {
    this.store.select(selectUser).subscribe((user) => {
      this.token = user?.accessToken ?? null;
    });
  }

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    if (this.token) {
      const authReq = req.clone({
        setHeaders: {
          Authorization: `Bearer ${this.token}`,
        },
      });
      return next.handle(authReq);
    }

    return next.handle(req);
  }
}
