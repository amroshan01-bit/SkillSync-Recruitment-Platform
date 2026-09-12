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
      .getMyApplications()
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