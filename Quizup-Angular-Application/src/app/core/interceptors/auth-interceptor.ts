import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
} from '@angular/common/http';
import { Observable, firstValueFrom, switchMap, take } from 'rxjs';
import { Store } from '@ngrx/store';
import { selectUser } from '../../store/auth/state/auth.selectors';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private store: Store) {
      console.log('AuthInterceptor constructed');

  }

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    console.log('AuthInterceptor: Intercepting request', req);
    return this.store.select(selectUser).pipe(
      take(1),
      switchMap((user: any) => {
        const token = user?.accessToken ?? null;
        if (token) {
          const authReq = req.clone({
            setHeaders: {
              Authorization: `Bearer ${token}`,
            },
          });
          return next.handle(authReq);
        }
        return next.handle(req);
      })
    );
  }
}
