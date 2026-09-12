import { HttpErrorResponse } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';

import { RankedApplicant } from '../../../core/models/job-application.model';
import { Vacancy } from '../../../core/models/vacancy.model';
import { AuthService } from '../../../core/services/auth.service';
import { JobApplicationService } from '../../../core/services/job-application.service';
import { VacancyService } from '../../../core/services/vacancy.service';

@Component({
  selector: 'app-employer-applications',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './employer-applications.component.html',
  styleUrl: './employer-applications.component.css',
})
export class EmployerApplicationsComponent implements OnInit {
  private readonly authService = inject(AuthService);
  private readonly jobApplicationService = inject(JobApplicationService);
  private readonly vacancyService = inject(VacancyService);
  private readonly router = inject(Router);

  readonly userName =
    this.authService.getUserName() ?? 'Employer';

  vacancies: Vacancy[] = [];
  applicants: RankedApplicant[] = [];

  selectedVacancyId = '';
  loadingVacancies = false;
  loadingApplicants = false;
  updatingApplicationId: string | null = null;

  errorMessage = '';
  successMessage = '';

  darkMode =
    localStorage.getItem('employerDarkMode') === 'true';

  ngOnInit(): void {
    this.loadVacancies();
  }

  loadVacancies(): void {
    this.loadingVacancies = true;
    this.errorMessage = '';

    this.vacancyService.getMine().subscribe({
      next: (vacancies) => {
        this.vacancies = vacancies;
        this.loadingVacancies = false;

        if (vacancies.length > 0) {
          this.selectVacancy(vacancies[0].id);
        }
      },
      error: (error: HttpErrorResponse) => {
        this.loadingVacancies = false;
        this.errorMessage = this.getErrorMessage(
          error,
          'Unable to load your vacancies.',
        );
      },
    });
  }

  selectVacancy(vacancyId: string): void {
    this.selectedVacancyId = vacancyId;
    this.loadApplicants();
  }

  loadApplicants(): void {
    if (!this.selectedVacancyId) {
      this.applicants = [];
      return;
    }

    this.loadingApplicants = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.jobApplicationService
      .getRankedApplicants(this.selectedVacancyId)
      .subscribe({
        next: (applicants) => {
          this.applicants = applicants;
          this.loadingApplicants = false;
        },
        error: (error: HttpErrorResponse) => {
          this.loadingApplicants = false;
          this.applicants = [];
          this.errorMessage = this.getErrorMessage(
            error,
            'Unable to load applicants.',
          );
        },
      });
  }

  updateStatus(
    applicant: RankedApplicant,
    status: string,
  ): void {
    if (!status || applicant.status === status) {
      return;
    }

    this.updatingApplicationId =
      applicant.applicationId;

    this.errorMessage = '';
    this.successMessage = '';

    this.jobApplicationService
      .updateStatus(
        applicant.applicationId,
        status,
      )
      .subscribe({
        next: () => {
          applicant.status = status;
          this.updatingApplicationId = null;
          this.successMessage =
            'Application status updated successfully.';
        },
        error: (error: HttpErrorResponse) => {
          this.updatingApplicationId = null;
          this.errorMessage = this.getErrorMessage(
            error,
            'Unable to update application status.',
          );
        },
      });
  }

  toggleDarkMode(): void {
    this.darkMode = !this.darkMode;

    localStorage.setItem(
      'employerDarkMode',
      String(this.darkMode),
    );
  }

  logout(): void {
    this.authService.logout();
    this.router.navigateByUrl('/login');
  }

  private getErrorMessage(
    error: HttpErrorResponse,
    fallbackMessage: string,
  ): string {
    if (
      typeof error.error === 'string' &&
      error.error.trim()
    ) {
      return error.error;
    }

    if (
      error.error &&
      typeof error.error.message === 'string'
    ) {
      return error.error.message;
    }

    return fallbackMessage;
  }
}