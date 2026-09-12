import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
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

  getCurrent(): Observable<JobSeekerProfile> {
    return this.http.get<JobSeekerProfile>(
      `${this.apiUrl}/me`,
    );
  }

  create(
    profile: CreateJobSeekerProfile,
  ): Observable<JobSeekerProfile> {
    return this.http.post<JobSeekerProfile>(
      `${this.apiUrl}/me`,
      profile,
    );
  }

  update(
    profile: UpdateJobSeekerProfile,
  ): Observable<void> {
    return this.http.put<void>(
      `${this.apiUrl}/me`,
      profile,
    );
  }

  deleteCurrent(): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/me`,
    );
  }
}