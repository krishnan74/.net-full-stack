// src/app/store/auth/auth.models.ts

export interface User {
  userId: number;
  username: string;
  role: string;
  accessToken: string;
  refreshToken: string;
  classGroupName?: string;
}

export interface AuthPayload {
  username: string;
  password: string;
}
