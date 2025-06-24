import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import * as AuthActions from './auth.actions';
import { AuthService } from '../auth.service';
import { catchError, map, mergeMap, of } from 'rxjs';
import { ApiResponse } from '../../../shared/models/api-response';
import { User } from '../auth.model';

@Injectable()
export class AuthEffects {
  constructor(private actions$: Actions, private authService: AuthService) {}

  login$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.login),
      mergeMap(({ payload }) =>
        this.authService.login(payload).pipe(
          map((loginResponse: ApiResponse<User>) => AuthActions.loginSuccess({ user: loginResponse.data })),
          catchError((err) =>
            of(AuthActions.loginFailure({ error: err.message }))
          )
        )
      )
    )
  );
}
