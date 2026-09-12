import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  CreateJobApplicationRequest,
  JobApplication,
  RankedApplicant,
  UpdateApplicationStatusRequest,
} from '../models/job-application.model';

@Injectable({
  providedIn: 'root',
})
export class JobApplicationService {
  private readonly apiUrl =
    'http://localhost:5075/api/job-applications';

  constructor(private readonly http: HttpClient) {}

  createApplication(
    request: CreateJobApplicationRequest
  ): Observable<JobApplication> {
    return this.http.post<JobApplication>(
      this.apiUrl,
      request
    );
  }

  getMyApplications(): Observable<JobApplication[]> {
    return this.http.get<JobApplication[]>(
      `${this.apiUrl}/job-seeker/me`
    );
  }

  getByVacancy(
    vacancyId: string
  ): Observable<JobApplication[]> {
    return this.http.get<JobApplication[]>(
      `${this.apiUrl}/vacancy/${vacancyId}`
    );
  }

  getRankedApplicants(
    vacancyId: string
  ): Observable<RankedApplicant[]> {
    return this.http.get<RankedApplicant[]>(
      `${this.apiUrl}/vacancy/${vacancyId}/ranked`
    );
  }

  updateStatus(
    applicationId: string,
    status: string
  ): Observable<void> {
    const request: UpdateApplicationStatusRequest = {
      status,
    };

    return this.http.patch<void>(
      `${this.apiUrl}/${applicationId}/status`,
      request
    );
  }
}