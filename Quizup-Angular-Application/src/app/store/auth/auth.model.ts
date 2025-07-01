// src/app/store/auth/auth.models.ts

export interface User {
  id: string;
  username: string;
  role: string;
  accessToken: string;
  refreshToken: string;
}

export interface AuthPayload {
  username: string;
  password: string;
}