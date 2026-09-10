import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';

import { environment } from '../../../environments/environment';
import {
  AuthResponse,
  CurrentUser,
  LoginRequest,
  RegisterRequest,
  UserRole,
} from '../models/auth.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = `${environment.apiUrl}/Auth`;
  private readonly storageKey = 'skillsync_auth';

  private readonly authSubject =
    new BehaviorSubject<AuthResponse | null>(
      this.loadStoredAuth(),
    );

  readonly auth$ = this.authSubject.asObservable();

  register(
    request: RegisterRequest,
  ): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(
        `${this.apiUrl}/register`,
        request,
      )
      .pipe(
        tap((response) => this.saveAuth(response)),
      );
  }

  login(
    request: LoginRequest,
  ): Observable<AuthResponse> {
    return this.http
      .post<AuthResponse>(
        `${this.apiUrl}/login`,
        request,
      )
      .pipe(
        tap((response) => this.saveAuth(response)),
      );
  }

  getCurrentUser(): Observable<CurrentUser> {
    return this.http.get<CurrentUser>(
      `${this.apiUrl}/me`,
    );
  }

  getToken(): string | null {
    return this.authSubject.value?.token ?? null;
  }

  getUserId(): string | null {
    return this.authSubject.value?.userId ?? null;
  }

  getUserName(): string | null {
    return this.authSubject.value?.fullName ?? null;
  }

  getRole(): UserRole | null {
    return this.authSubject.value?.role ?? null;
  }

  isAuthenticated(): boolean {
    const auth = this.authSubject.value;

    if (!auth) {
      return false;
    }

    const expiryTime = Date.parse(auth.expiresAtUtc);

    if (
      Number.isNaN(expiryTime) ||
      expiryTime <= Date.now()
    ) {
      this.logout();
      return false;
    }

    return true;
  }

  logout(): void {
    localStorage.removeItem(this.storageKey);
    this.authSubject.next(null);
  }

  private saveAuth(auth: AuthResponse): void {
    localStorage.setItem(
      this.storageKey,
      JSON.stringify(auth),
    );

    this.authSubject.next(auth);
  }

  private loadStoredAuth(): AuthResponse | null {
    const storedAuth = localStorage.getItem(
      this.storageKey,
    );

    if (!storedAuth) {
      return null;
    }

    try {
      const auth = JSON.parse(
        storedAuth,
      ) as AuthResponse;

      const expiryTime = Date.parse(auth.expiresAtUtc);

      if (
        !auth.token ||
        Number.isNaN(expiryTime) ||
        expiryTime <= Date.now()
      ) {
        localStorage.removeItem(this.storageKey);
        return null;
      }

      return auth;
    } catch {
      localStorage.removeItem(this.storageKey);
      return null;
    }
  }
}
