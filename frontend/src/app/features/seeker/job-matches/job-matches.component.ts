import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { MatchResult } from '../../../core/models/match-result.model';
import { JobApplicationService } from '../../../core/services/job-application.service';
import { MatchingService } from '../../../core/services/matching.service';

@Component({
  selector: 'app-job-matches',
  imports: [
    CommonModule,
    FormsModule,
  ],
  templateUrl: './job-matches.component.html',
  styleUrl: './job-matches.component.css',
})
export class JobMatchesComponent implements OnInit {
  private readonly userId =
    '11111111-1111-1111-1111-111111111111';

  matches: MatchResult[] = [];
  selectedMatch: MatchResult | null = null;

  coverLetter = '';

  isLoading = false;
  isApplying = false;

  successMessage = '';
  errorMessage = '';

  constructor(
    private readonly matchingService: MatchingService,
    private readonly applicationService: JobApplicationService
  ) {}

  ngOnInit(): void {
    this.loadMatches();
  }

  loadMatches(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.matchingService.getMatches(this.userId).subscribe({
      next: (matches: MatchResult[]) => {
        this.matches = matches;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage =
          'Unable to load job matches.';
        this.isLoading = false;
      },
    });
  }

  openApplication(match: MatchResult): void {
    if (match.hasApplied) {
      return;
    }

    this.selectedMatch = match;
    this.coverLetter = '';
    this.successMessage = '';
    this.errorMessage = '';
  }

  closeApplication(): void {
    if (this.isApplying) {
      return;
    }

    this.selectedMatch = null;
    this.coverLetter = '';
  }

  submitApplication(): void {
    if (!this.selectedMatch) {
      return;
    }

    const coverLetter = this.coverLetter.trim();

    if (!coverLetter) {
      this.errorMessage =
        'Please enter your cover letter.';
      return;
    }

    this.isApplying = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.applicationService
      .createApplication({
        vacancyId: this.selectedMatch.vacancyId,
        jobSeekerUserId: this.userId,
        coverLetter,
      })
      .subscribe({
        next: () => {
          this.successMessage =
            'Your application was submitted successfully.';

          this.selectedMatch = null;
          this.coverLetter = '';
          this.isApplying = false;

          this.loadMatches();
        },
        error: (error: HttpErrorResponse) => {
          this.errorMessage =
            error.error?.message ??
            'Unable to submit your application.';

          this.isApplying = false;
        },
      });
  }

  matchLevel(score: number): string {
    if (score >= 80) {
      return 'Excellent Match';
    }

    if (score >= 60) {
      return 'Good Match';
    }

    return 'Potential Match';
  }
}