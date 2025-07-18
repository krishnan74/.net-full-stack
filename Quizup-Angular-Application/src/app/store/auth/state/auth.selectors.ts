import { createFeatureSelector, createSelector } from '@ngrx/store';
import { AuthState } from './auth.reducer';

export const selectAuthState = createFeatureSelector<AuthState>('auth');

export const selectUser = createSelector(
  selectAuthState,
  (state) => state.user
);
export const selectAuthError = createSelector(
  selectAuthState,
  (state) => state.error
);
export const selectIsAuthenticated = createSelector(
  selectUser,
  (user) => !!user
);
