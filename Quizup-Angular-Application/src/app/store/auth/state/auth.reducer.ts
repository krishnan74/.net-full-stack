
import { createReducer, on } from '@ngrx/store';
import * as AuthActions from './auth.actions';
import { User } from '../auth.model';

export interface AuthState {
  user: User | null;
  error: string | null;
}

export const initialState: AuthState = {
  user: null,
  error: null,
};

export const authReducer = createReducer(
  initialState,
  on(AuthActions.loginSuccess, (state, { user }) => ({
    ...state,
    user,
    error: null,
  })),
  on(AuthActions.loginFailure, (state, { error }) => ({
    ...state,
    user: null,
    error,
  })),
  on(AuthActions.logout, () => initialState)
);
