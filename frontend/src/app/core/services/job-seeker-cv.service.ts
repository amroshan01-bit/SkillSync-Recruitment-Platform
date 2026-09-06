import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { JobSeekerCv } from '../models/job-seeker-cv.model';

@Injectable({ providedIn: 'root' })
export class JobSeekerCvService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/job-seeker-cvs`;

  getByUserId(userId: string): Observable<JobSeekerCv> {
    return this.http.get<JobSeekerCv>(`${this.apiUrl}/user/${userId}`);
  }

  upload(userId: string, file: File): Observable<JobSeekerCv> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<JobSeekerCv>(`${this.apiUrl}/upload/${userId}`, formData);
  }

  download(userId: string): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/download/${userId}`, { responseType: 'blob' });
  }

  delete(userId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/user/${userId}`);
  }
}
