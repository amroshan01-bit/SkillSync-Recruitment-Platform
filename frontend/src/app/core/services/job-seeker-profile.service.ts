import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import {
  CreateJobSeekerProfile,
  JobSeekerProfile,
  UpdateJobSeekerProfile,
} from '../models/job-seeker-profile.model';

@Injectable({
  providedIn: 'root',
})
export class JobSeekerProfileService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl =
    `${environment.apiUrl}/job-seeker-profiles`;

  getAll(): Observable<JobSeekerProfile[]> {
    return this.http.get<JobSeekerProfile[]>(this.apiUrl);
  }

  getById(id: string): Observable<JobSeekerProfile> {
    return this.http.get<JobSeekerProfile>(
      `${this.apiUrl}/${id}`,
    );
  }

  getByUserId(userId: string): Observable<JobSeekerProfile> {
    return this.http.get<JobSeekerProfile>(
      `${this.apiUrl}/user/${userId}`,
    );
  }

  create(
    profile: CreateJobSeekerProfile,
  ): Observable<JobSeekerProfile> {
    return this.http.post<JobSeekerProfile>(
      this.apiUrl,
      profile,
    );
  }

  update(
    id: string,
    profile: UpdateJobSeekerProfile,
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