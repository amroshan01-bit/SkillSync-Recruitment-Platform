import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';

import { JobApplication } from '../../../core/models/job-application.model';
import { JobApplicationService } from '../../../core/services/job-application.service';

@Component({
  selector: 'app-my-applications',
  imports: [CommonModule],
  templateUrl: './my-applications.component.html',
  styleUrl: './my-applications.component.css',
})
export class MyApplicationsComponent implements OnInit {
  private readonly userId =
    '11111111-1111-1111-1111-111111111111';

  applications: JobApplication[] = [];

  isLoading = false;
  errorMessage = '';

  constructor(
    private readonly applicationService:
      JobApplicationService
  ) {}

  ngOnInit(): void {
    this.loadApplications();
  }

  loadApplications(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.applicationService
      .getByJobSeeker(this.userId)
      .subscribe({
        next: (applications: JobApplication[]) => {
          this.applications = applications;
          this.isLoading = false;
        },
        error: () => {
          this.errorMessage =
            'Unable to load your applications.';
          this.isLoading = false;
        },
      });
  }

  getStatusClass(status: string): string {
    return status
      .trim()
      .toLowerCase()
      .replace(/\s+/g, '-');
  }
}