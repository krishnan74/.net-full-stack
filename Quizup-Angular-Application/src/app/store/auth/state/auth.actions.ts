
import { createAction, props } from '@ngrx/store';
import { User, AuthPayload } from '../auth.model';

export const login = createAction(
  '[Auth] Login',
  props<{ payload: AuthPayload }>()
);
export const loginSuccess = createAction(
  '[Auth] Login Success',
  props<{ user: User }>()
);
export const loginFailure = createAction(
  '[Auth] Login Failure',
  props<{ error: string }>()
);
export const logout = createAction('[Auth] Logout');
