import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, inject } from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

import {
  CreateEmployerProfile,
  EmployerProfile,
  UpdateEmployerProfile,
} from '../../../core/models/employer-profile.model';
import { EmployerProfileService } from '../../../core/services/employer-profile.service';

@Component({
  selector: 'app-employer-profile',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './employer-profile.component.html',
  styleUrl: './employer-profile.component.css',
})
export class EmployerProfileComponent implements OnInit {
  private readonly formBuilder = inject(FormBuilder);
  private readonly profileService = inject(
    EmployerProfileService,
  );

  readonly userId =
    '11111111-1111-1111-1111-111111111111';

  profile: EmployerProfile | null = null;
  loading = false;
  saving = false;
  darkMode = false;
  errorMessage = '';
  successMessage = '';

  profileForm = this.formBuilder.nonNullable.group({
    companyName: [
      '',
      [
        Validators.required,
        Validators.maxLength(200),
      ],
    ],
    industry: [
      '',
      [
        Validators.required,
        Validators.maxLength(100),
      ],
    ],
    companySize: [
      '',
      [
        Validators.required,
        Validators.maxLength(50),
      ],
    ],
    website: [
      '',
      Validators.maxLength(250),
    ],
    location: [
      '',
      [
        Validators.required,
        Validators.maxLength(200),
      ],
    ],
    aboutCompany: [
      '',
      [
        Validators.required,
        Validators.maxLength(2000),
      ],
    ],
    logoPath: [
      '',
      Validators.maxLength(500),
    ],
    contactPerson: [
      '',
      [
        Validators.required,
        Validators.maxLength(150),
      ],
    ],
    emailAddress: [
      '',
      [
        Validators.required,
        Validators.email,
        Validators.maxLength(200),
      ],
    ],
    phoneNumber: [
      '',
      [
        Validators.required,
        Validators.maxLength(30),
      ],
    ],
  });

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile(): void {
    this.loading = true;
    this.errorMessage = '';

    this.profileService
      .getByUserId(this.userId)
      .subscribe({
        next: (profile) => {
          this.profile = profile;

          this.profileForm.patchValue({
            companyName: profile.companyName,
            industry: profile.industry,
            companySize: profile.companySize,
            website: profile.website ?? '',
            location: profile.location,
            aboutCompany: profile.aboutCompany,
            logoPath: profile.logoPath ?? '',
            contactPerson: profile.contactPerson,
            emailAddress: profile.emailAddress,
            phoneNumber: profile.phoneNumber,
          });

          this.loading = false;
        },
        error: (error: HttpErrorResponse) => {
          this.loading = false;

          if (error.status === 404) {
            this.profile = null;
            return;
          }

          this.errorMessage =
            'Unable to load the employer profile.';
        },
      });
  }

  saveProfile(): void {
    if (this.profileForm.invalid) {
      this.profileForm.markAllAsTouched();
      return;
    }

    this.saving = true;
    this.errorMessage = '';
    this.successMessage = '';

    const value = this.profileForm.getRawValue();

    const formValue = {
      ...value,
      website: value.website || null,
      logoPath: value.logoPath || null,
    };

    if (this.profile) {
      const updateData: UpdateEmployerProfile =
        formValue;

      this.updateProfile(updateData);
      return;
    }

    const createData: CreateEmployerProfile = {
      userId: this.userId,
      ...formValue,
    };

    this.createProfile(createData);
  }

  private createProfile(
    createData: CreateEmployerProfile,
  ): void {
    this.profileService.create(createData).subscribe({
      next: (profile) => {
        this.profile = profile;
        this.saving = false;
        this.successMessage =
          'Employer profile created successfully.';
      },
      error: () => {
        this.saving = false;
        this.errorMessage =
          'Unable to create the employer profile.';
      },
    });
  }

  private updateProfile(
    updateData: UpdateEmployerProfile,
  ): void {
    if (!this.profile) {
      return;
    }

    this.profileService
      .update(this.profile.id, updateData)
      .subscribe({
        next: () => {
          this.saving = false;
          this.successMessage =
            'Employer profile updated successfully.';
          this.loadProfile();
        },
        error: () => {
          this.saving = false;
          this.errorMessage =
            'Unable to update the employer profile.';
        },
      });
  }

  toggleDarkMode(): void {
    this.darkMode = !this.darkMode;
  }
}