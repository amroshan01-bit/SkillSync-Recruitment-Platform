import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import {
  Router,
  RouterLink,
} from '@angular/router';

import { JobApplication } from '../../../core/models/job-application.model';
import { AuthService } from '../../../core/services/auth.service';
import { JobApplicationService } from '../../../core/services/job-application.service';

@Component({
  selector: 'app-my-applications',
  imports: [
    CommonModule,
    RouterLink,
  ],
  templateUrl: './my-applications.component.html',
  styleUrl: './my-applications.component.css',
})
export class MyApplicationsComponent implements OnInit {
  applications: JobApplication[] = [];

  isLoading = false;
  errorMessage = '';

  darkMode = false;
  userName = 'User';

  constructor(
    private readonly applicationService: JobApplicationService,
    private readonly authService: AuthService,
    private readonly router: Router,
  ) {
    this.userName =
      this.authService.getUserName() ?? 'User';
  }

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

  goBack(): void {
    window.history.back();
  }

  logout(): void {
    this.authService.logout();
    this.router.navigateByUrl('/login');
  }

  toggleDarkMode(): void {
    this.darkMode = !this.darkMode;
  }
}