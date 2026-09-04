import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, inject } from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

import {
  CreateJobSeekerProfile,
  JobSeekerProfile,
  UpdateJobSeekerProfile,
} from '../../../core/models/job-seeker-profile.model';
import { JobSeekerProfileService } from '../../../core/services/job-seeker-profile.service';
import { JobSeekerCvComponent } from '../job-seeker-cv/job-seeker-cv.component';

@Component({
  selector: 'app-job-seeker-profile',
  standalone: true,
  imports: [
  ReactiveFormsModule,
  JobSeekerCvComponent,
],
  templateUrl: './job-seeker-profile.component.html',
  styleUrl: './job-seeker-profile.component.css',
})
export class JobSeekerProfileComponent implements OnInit {
  // Form உருவாக்க FormBuilder object பெறுகிறோம்.
  private readonly formBuilder = inject(FormBuilder);

  // Backend API methods பயன்படுத்த Service object பெறுகிறோம்.
  private readonly profileService = inject(
    JobSeekerProfileService,
  );

  // Authentication முடியும்வரை பயன்படுத்தும் temporary User ID.
  readonly userId =
    '11111111-1111-1111-1111-111111111111';

  // Backend-லிருந்து கிடைக்கும் profile-ஐ வைத்திருக்கும்.
  profile: JobSeekerProfile | null = null;

  // Page loading நிலையை வைத்திருக்கும்.
  loading = false;

  // Save operation நடக்கிறதா என்பதை வைத்திருக்கும்.
  saving = false;

  // Error message காட்டப் பயன்படும்.
  errorMessage = '';

  // Success message காட்டப் பயன்படும்.
  successMessage = '';

  // Light Mode அல்லது Dark Mode என்பதை வைத்திருக்கும்.
  darkMode = false;

  // Profile form மற்றும் அதன் validation rules.
  profileForm = this.formBuilder.nonNullable.group({
    fullName: [
      '',
      [
        Validators.required,
        Validators.maxLength(150),
      ],
    ],

    professionalTitle: [
      '',
      Validators.maxLength(150),
    ],

    bio: [
      '',
      Validators.maxLength(1000),
    ],

    location: [
      '',
      Validators.maxLength(150),
    ],

    phoneNumber: [
      '',
      Validators.maxLength(30),
    ],

    yearsOfExperience: [
      0,
      [
        Validators.required,
        Validators.min(0),
        Validators.max(60),
      ],
    ],

    highestQualification: [
      '',
      Validators.maxLength(200),
    ],
  });

  // Page திறந்தவுடன் profile data load செய்யும்.
  ngOnInit(): void {
    this.loadProfile();
  }

  // User ID மூலம் Backend-லிருந்து profile பெறும்.
  loadProfile(): void {
    this.loading = true;
    this.errorMessage = '';

    this.profileService
      .getByUserId(this.userId)
      .subscribe({
        // API success ஆனால் form-ல் data நிரப்பும்.
        next: (profile) => {
          this.profile = profile;

          this.profileForm.patchValue({
            fullName: profile.fullName,
            professionalTitle:
              profile.professionalTitle ?? '',
            bio: profile.bio ?? '',
            location: profile.location ?? '',
            phoneNumber: profile.phoneNumber ?? '',
            yearsOfExperience:
              profile.yearsOfExperience,
            highestQualification:
              profile.highestQualification ?? '',
          });

          this.loading = false;
        },

        // API error வந்தால் அதைக் கையாளும்.
        error: (error: HttpErrorResponse) => {
          this.loading = false;

          // 404 என்றால் இந்த user-க்கு profile இன்னும் இல்லை.
          if (error.status === 404) {
            this.profile = null;
            return;
          }

          this.errorMessage =
            'Unable to load the profile.';
        },
      });
  }

  // Form submit செய்தால் create அல்லது update செய்யும்.
  saveProfile(): void {
    // Form invalid என்றால் API request அனுப்பாது.
    if (this.profileForm.invalid) {
      this.profileForm.markAllAsTouched();
      return;
    }

    this.saving = true;
    this.errorMessage = '';
    this.successMessage = '';

    // Form-ல் user கொடுத்த எல்லா values-ஐ பெறும்.
    const formValue = this.profileForm.getRawValue();

    // Profile ஏற்கனவே இருந்தால் update செய்யும்.
    if (this.profile) {
      const updateData: UpdateJobSeekerProfile =
        formValue;

      this.updateProfile(updateData);
      return;
    }

    // Profile இல்லையென்றால் புதிய profile உருவாக்கும்.
    const createData: CreateJobSeekerProfile = {
      userId: this.userId,
      ...formValue,
    };

    this.createProfile(createData);
  }

  // புதிய profile-ஐ Backend Database-ல் உருவாக்கும்.
  private createProfile(
    createData: CreateJobSeekerProfile,
  ): void {
    this.profileService.create(createData).subscribe({
      next: (profile) => {
        this.profile = profile;
        this.saving = false;
        this.successMessage =
          'Profile created successfully.';
      },
      error: () => {
        this.saving = false;
        this.errorMessage =
          'Unable to create the profile.';
      },
    });
  }

  // ஏற்கனவே இருக்கும் profile-ஐ update செய்யும்.
  private updateProfile(
    updateData: UpdateJobSeekerProfile,
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
            'Profile updated successfully.';

          // Updated data-ஐ மீண்டும் Backend-லிருந்து பெறும்.
          this.loadProfile();
        },
        error: () => {
          this.saving = false;
          this.errorMessage =
            'Unable to update the profile.';
        },
      });
  }

  // Light Mode மற்றும் Dark Mode இடையே மாற்றும்.
  toggleDarkMode(): void {
    this.darkMode = !this.darkMode;
  }
}