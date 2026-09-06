import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import {
  CreateVacancy,
  UpdateVacancy,
  Vacancy,
} from '../models/vacancy.model';

@Injectable({
  providedIn: 'root',
})
export class VacancyService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl =
    `${environment.apiUrl}/vacancies`;

  getAll(status?: string, search?: string): Observable<Vacancy[]> {
    let params = new HttpParams();

    if (status) {
      params = params.set('status', status);
    }

    if (search) {
      params = params.set('search', search);
    }

    return this.http.get<Vacancy[]>(this.apiUrl, { params });
  }

  getById(id: string): Observable<Vacancy> {
    return this.http.get<Vacancy>(
      `${this.apiUrl}/${id}`,
    );
  }

  getByEmployerProfileId(
    employerProfileId: string,
    status?: string,
  ): Observable<Vacancy[]> {
    let params = new HttpParams();

    if (status) {
      params = params.set('status', status);
    }

    return this.http.get<Vacancy[]>(
      `${this.apiUrl}/employer/${employerProfileId}`,
      { params },
    );
  }

  create(vacancy: CreateVacancy): Observable<Vacancy> {
    return this.http.post<Vacancy>(
      this.apiUrl,
      vacancy,
    );
  }

  update(
    id: string,
    vacancy: UpdateVacancy,
  ): Observable<void> {
    return this.http.put<void>(
      `${this.apiUrl}/${id}`,
      vacancy,
    );
  }

  close(id: string): Observable<void> {
    return this.http.patch<void>(
      `${this.apiUrl}/${id}/close`,
      null,
    );
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/${id}`,
    );
  }
}