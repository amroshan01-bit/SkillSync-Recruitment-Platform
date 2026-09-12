import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  MatchResult,
  UpdateJobSeekerSkillsRequest,
} from '../models/match-result.model';

@Injectable({
  providedIn: 'root',
})
export class MatchingService {
  private readonly apiUrl =
    'http://localhost:5075/api/matching';

  constructor(private readonly http: HttpClient) {}

  getSkills(): Observable<string[]> {
    return this.http.get<string[]>(
      `${this.apiUrl}/skills`
    );
  }

  updateSkills(
    request: UpdateJobSeekerSkillsRequest
  ): Observable<void> {
    return this.http.put<void>(
      `${this.apiUrl}/skills`,
      request
    );
  }

  getMatches(): Observable<MatchResult[]> {
    return this.http.get<MatchResult[]>(
      `${this.apiUrl}/results`
    );
  }
}