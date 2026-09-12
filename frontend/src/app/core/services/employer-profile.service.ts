import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
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
    `${environment.apiUrl}/employer-profiles/me`;

  getCurrent(): Observable<EmployerProfile> {
    return this.http.get<EmployerProfile>(this.apiUrl);
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
    profile: UpdateEmployerProfile,
  ): Observable<void> {
    return this.http.put<void>(
      this.apiUrl,
      profile,
    );
  }

  delete(): Observable<void> {
    return this.http.delete<void>(this.apiUrl);
  }
}