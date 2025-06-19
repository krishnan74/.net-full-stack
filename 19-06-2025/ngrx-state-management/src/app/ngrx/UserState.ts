import { User } from '../models/User.ts';


export interface UserState {
  users: User[];
  loading: boolean;
  error: string | null;
}

export const initialUserState: UserState = {
  users: [],
  loading: false,
  error: null
};