import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { JobSeekerCv } from '../models/job-seeker-cv.model';

@Injectable({ providedIn: 'root' })
export class JobSeekerCvService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl =
    `${environment.apiUrl}/job-seeker-cvs`;

  getCurrent(): Observable<JobSeekerCv> {
    return this.http.get<JobSeekerCv>(
      `${this.apiUrl}/me`,
    );
  }

  upload(file: File): Observable<JobSeekerCv> {
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post<JobSeekerCv>(
      `${this.apiUrl}/me/upload`,
      formData,
    );
  }

  download(): Observable<Blob> {
    return this.http.get(
      `${this.apiUrl}/me/download`,
      {
        responseType: 'blob',
      },
    );
  }

  deleteCurrent(): Observable<void> {
    return this.http.delete<void>(
      `${this.apiUrl}/me`,
    );
  }
}