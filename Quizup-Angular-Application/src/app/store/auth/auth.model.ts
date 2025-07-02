// src/app/store/auth/auth.models.ts

export interface User {
  id: number;
  username: string;
  role: string;
  accessToken: string;
  refreshToken: string;
}

export interface AuthPayload {
  username: string;
  password: string;
}
