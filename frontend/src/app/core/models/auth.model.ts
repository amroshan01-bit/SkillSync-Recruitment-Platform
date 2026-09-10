export type UserRole = 'JobSeeker' | 'Employer' | 'Admin';

export interface RegisterRequest {
  fullName: string;
  email: string;
  password: string;
  confirmPassword: string;
  role: UserRole;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  expiresAtUtc: string;
  userId: string;
  fullName: string;
  email: string;
  role: UserRole;
}

export interface CurrentUser {
  userId: string;
  fullName: string;
  email: string;
  role: UserRole;
}