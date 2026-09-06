import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import {
  CreateEmployerProfile,
  EmployerProfile,
  UpdateEmployerProfile,
} from '../models/employer-profile.model';

@Injectable({
  providedIn: 'root',
})
export class EmployerProfileService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl =
    `${environment.apiUrl}/employer-profiles`;

  getAll(): Observable<EmployerProfile[]> {
    return this.http.get<EmployerProfile[]>(this.apiUrl);
  }

  getById(id: string): Observable<EmployerProfile> {
    return this.http.get<EmployerProfile>(
      `${this.apiUrl}/${id}`,
    );
  }

  getByUserId(userId: string): Observable<EmployerProfile> {
    return this.http.get<EmployerProfile>(
      `${this.apiUrl}/user/${userId}`,
    );
  }

  create(
    profile: CreateEmployerProfile,
  ): Observable<EmployerProfile> {
    return this.http.post<EmployerProfile>(
      this.apiUrl,
      profile,
    );
  }

  update(
    id: string,
    profile: UpdateEmployerProfile,
  ): Observable<void> {
    return this.http.put<void>(
      `${this.apiUrl}/${id}`,
      profile,
    );
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/${id}`,
    );
  }
}