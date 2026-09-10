import { HttpErrorResponse } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';

import {
  CreateVacancy,
  UpdateVacancy,
  Vacancy,
} from '../../../core/models/vacancy.model';
import { AuthService } from '../../../core/services/auth.service';
import { EmployerProfileService } from '../../../core/services/employer-profile.service';
import { VacancyService } from '../../../core/services/vacancy.service';

@Component({
  selector: 'app-vacancy-management',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './vacancy-management.component.html',
  styleUrl: './vacancy-management.component.css',
})
export class VacancyManagementComponent implements OnInit {
  private readonly formBuilder = inject(FormBuilder);
  private readonly employerProfileService = inject(
    EmployerProfileService,
  );
  private readonly vacancyService = inject(VacancyService);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  readonly userId = this.authService.getUserId() ?? '';
  readonly userName = this.authService.getUserName() ?? 'User';

  employerProfileId = '';
  companyName = '';
  vacancies: Vacancy[] = [];
  editingVacancyId: string | null = null;

  loading = false;
  saving = false;
  darkMode = false;
  showForm = false;
  selectedStatus = '';
  errorMessage = '';
  successMessage = '';

  vacancyForm = this.formBuilder.nonNullable.group({
    jobTitle: [
      '',
      [
        Validators.required,
        Validators.maxLength(150),
      ],
    ],
    department: [
      '',
      [
        Validators.required,
        Validators.maxLength(100),
      ],
    ],
    location: [
      '',
      [
        Validators.required,
        Validators.maxLength(150),
      ],
    ],
    workplaceType: [
      '',
      Validators.required,
    ],
    employmentType: [
      '',
      Validators.required,
    ],
    experienceLevel: [
      '',
      Validators.required,
    ],
    jobDescription: [
      '',
      [
        Validators.required,
        Validators.maxLength(5000),
      ],
    ],
    requirements: [
      '',
      [
        Validators.required,
        Validators.maxLength(5000),
      ],
    ],
    requiredSkills: [
      '',
      [
        Validators.required,
        Validators.maxLength(1000),
      ],
    ],
    minimumSalary: [
      0,
      [
        Validators.required,
        Validators.min(0),
      ],
    ],
    maximumSalary: [
      0,
      [
        Validators.required,
        Validators.min(0),
      ],
    ],
    currency: [
      'LKR',
      [
        Validators.required,
        Validators.maxLength(10),
      ],
    ],
    applicationClosingDate: [
      '',
      Validators.required,
    ],
    numberOfOpenings: [
      1,
      [
        Validators.required,
        Validators.min(1),
        Validators.max(1000),
      ],
    ],
    status: [
      'Draft',
      Validators.required,
    ],
  });

  ngOnInit(): void {
    if (!this.userId) {
      this.logout();
      return;
    }

    this.loadEmployerProfile();
  }

  loadEmployerProfile(): void {
    this.loading = true;
    this.errorMessage = '';

    this.employerProfileService
      .getByUserId(this.userId)
      .subscribe({
        next: (profile) => {
          this.employerProfileId = profile.id;
          this.companyName = profile.companyName;
          this.loadVacancies();
        },
        error: () => {
          this.loading = false;
          this.errorMessage =
            'Create an employer profile before managing vacancies.';
        },
      });
  }

  loadVacancies(): void {
    if (!this.employerProfileId) {
      return;
    }

    this.loading = true;
    this.errorMessage = '';

    this.vacancyService
      .getByEmployerProfileId(
        this.employerProfileId,
        this.selectedStatus || undefined,
      )
      .subscribe({
        next: (vacancies) => {
          this.vacancies = vacancies;
          this.loading = false;
        },
        error: () => {
          this.loading = false;
          this.errorMessage =
            'Unable to load vacancies.';
        },
      });
  }

  openCreateForm(): void {
    this.editingVacancyId = null;
    this.successMessage = '';
    this.errorMessage = '';
    this.showForm = true;

    this.vacancyForm.reset({
      jobTitle: '',
      department: '',
      location: '',
      workplaceType: '',
      employmentType: '',
      experienceLevel: '',
      jobDescription: '',
      requirements: '',
      requiredSkills: '',
      minimumSalary: 0,
      maximumSalary: 0,
      currency: 'LKR',
      applicationClosingDate: '',
      numberOfOpenings: 1,
      status: 'Draft',
    });
  }

  editVacancy(vacancy: Vacancy): void {
    if (vacancy.status === 'Closed') {
      this.errorMessage =
        'A closed vacancy cannot be edited.';
      return;
    }

    this.editingVacancyId = vacancy.id;
    this.successMessage = '';
    this.errorMessage = '';
    this.showForm = true;

    this.vacancyForm.patchValue({
      jobTitle: vacancy.jobTitle,
      department: vacancy.department,
      location: vacancy.location,
      workplaceType: vacancy.workplaceType,
      employmentType: vacancy.employmentType,
      experienceLevel: vacancy.experienceLevel,
      jobDescription: vacancy.jobDescription,
      requirements: vacancy.requirements,
      requiredSkills: vacancy.requiredSkills,
      minimumSalary: vacancy.minimumSalary,
      maximumSalary: vacancy.maximumSalary,
      currency: vacancy.currency,
      applicationClosingDate:
        vacancy.applicationClosingDate.slice(0, 16),
      numberOfOpenings: vacancy.numberOfOpenings,
      status: vacancy.status,
    });

    window.scrollTo({
      top: 0,
      behavior: 'smooth',
    });
  }

  cancelForm(): void {
    this.showForm = false;
    this.editingVacancyId = null;
    this.vacancyForm.reset();
  }

  saveVacancy(): void {
    if (this.vacancyForm.invalid) {
      this.vacancyForm.markAllAsTouched();
      return;
    }

    const value = this.vacancyForm.getRawValue();

    if (value.maximumSalary < value.minimumSalary) {
      this.errorMessage =
        'Maximum salary must be greater than minimum salary.';
      return;
    }

    if (!this.employerProfileId) {
      this.errorMessage =
        'Employer profile was not found.';
      return;
    }

    this.saving = true;
    this.errorMessage = '';
    this.successMessage = '';

    const vacancyData: UpdateVacancy = {
      ...value,
      applicationClosingDate:
        new Date(value.applicationClosingDate).toISOString(),
    };

    if (this.editingVacancyId) {
      this.updateVacancy(
        this.editingVacancyId,
        vacancyData,
      );
      return;
    }

    const createData: CreateVacancy = {
      employerProfileId: this.employerProfileId,
      ...vacancyData,
    };

    this.createVacancy(createData);
  }

  private createVacancy(
    createData: CreateVacancy,
  ): void {
    this.vacancyService.create(createData).subscribe({
      next: () => {
        this.saving = false;
        this.showForm = false;
        this.successMessage =
          'Vacancy created successfully.';
        this.loadVacancies();
      },
      error: (error: HttpErrorResponse) => {
        this.saving = false;
        this.errorMessage =
          this.getErrorMessage(
            error,
            'Unable to create the vacancy.',
          );
      },
    });
  }

  private updateVacancy(
    id: string,
    updateData: UpdateVacancy,
  ): void {
    this.vacancyService.update(id, updateData).subscribe({
      next: () => {
        this.saving = false;
        this.showForm = false;
        this.editingVacancyId = null;
        this.successMessage =
          'Vacancy updated successfully.';
        this.loadVacancies();
      },
      error: (error: HttpErrorResponse) => {
        this.saving = false;
        this.errorMessage =
          this.getErrorMessage(
            error,
            'Unable to update the vacancy.',
          );
      },
    });
  }

  closeVacancy(vacancy: Vacancy): void {
    const confirmed = window.confirm(
      `Close the vacancy "${vacancy.jobTitle}"?`,
    );

    if (!confirmed) {
      return;
    }

    this.errorMessage = '';
    this.successMessage = '';

    this.vacancyService.close(vacancy.id).subscribe({
      next: () => {
        this.successMessage =
          'Vacancy closed successfully.';
        this.loadVacancies();
      },
      error: () => {
        this.errorMessage =
          'Unable to close the vacancy.';
      },
    });
  }

  deleteVacancy(vacancy: Vacancy): void {
    const confirmed = window.confirm(
      `Delete the vacancy "${vacancy.jobTitle}" permanently?`,
    );

    if (!confirmed) {
      return;
    }

    this.errorMessage = '';
    this.successMessage = '';

    this.vacancyService.delete(vacancy.id).subscribe({
      next: () => {
        this.successMessage =
          'Vacancy deleted successfully.';
        this.loadVacancies();
      },
      error: () => {
        this.errorMessage =
          'Unable to delete the vacancy.';
      },
    });
  }

  changeStatusFilter(status: string): void {
    this.selectedStatus = status;
    this.loadVacancies();
  }

  toggleDarkMode(): void {
    this.darkMode = !this.darkMode;
  }

  logout(): void {
    this.authService.logout();
    this.router.navigateByUrl('/login');
  }

  private getErrorMessage(
    error: HttpErrorResponse,
    fallbackMessage: string,
  ): string {
    if (typeof error.error === 'string') {
      return error.error;
    }

    return fallbackMessage;
  }
}
