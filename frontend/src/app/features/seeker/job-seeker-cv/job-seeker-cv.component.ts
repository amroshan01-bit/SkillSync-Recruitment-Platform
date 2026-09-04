import { HttpErrorResponse } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';

import { JobSeekerCv } from '../../../core/models/job-seeker-cv.model';
import { JobSeekerCvService } from '../../../core/services/job-seeker-cv.service'

@Component({
  selector: 'app-job-seeker-cv',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './job-seeker-cv.component.html',
  styleUrl: './job-seeker-cv.component.css',
})
export class JobSeekerCvComponent implements OnInit {
  // Gets the CV service.
  private readonly cvService = inject(JobSeekerCvService);

  // Temporary user ID until authentication is added.
  readonly userId = '11111111-1111-1111-1111-111111111111';

  // Stores the uploaded CV information.
  cv: JobSeekerCv | null = null;

  // Stores the file selected by the user.
  selectedFile: File | null = null;

  // Shows the initial loading state.
  loading = false;

  // Shows the upload progress state.
  uploading = false;

  // Shows the delete progress state.
  deleting = false;

  // Stores an error message.
  errorMessage = '';

  // Stores a success message.
  successMessage = '';

  // Allowed CV file types.
  private readonly allowedExtensions = ['pdf', 'doc', 'docx'];

  // Maximum file size is 5 MB.
  private readonly maximumFileSize = 5 * 1024 * 1024;

  // Loads the CV when the component opens.
  ngOnInit(): void {
    this.loadCv();
  }

  // Gets the current user's CV from the backend.
  loadCv(): void {
    this.loading = true;
    this.errorMessage = '';

    this.cvService.getByUserId(this.userId).subscribe({
      next: (cv) => {
        this.cv = cv;
        this.loading = false;
      },

      error: (error: HttpErrorResponse) => {
        this.loading = false;

        // A 404 response means the user has no CV yet.
        if (error.status === 404) {
          this.cv = null;
          return;
        }

        this.errorMessage = 'Unable to load the CV.';
      },
    });
  }

  // Reads and validates the selected file.
  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    const file = input.files?.[0];

    this.errorMessage = '';
    this.successMessage = '';

    if (!file) {
      this.selectedFile = null;
      return;
    }

    const extension = file.name
      .split('.')
      .pop()
      ?.toLowerCase();

    // Checks whether the file format is allowed.
    if (
      !extension ||
      !this.allowedExtensions.includes(extension)
    ) {
      this.selectedFile = null;
      this.errorMessage =
        'Only PDF, DOC and DOCX files are allowed.';
      input.value = '';
      return;
    }

    // Checks whether the file is larger than 5 MB.
    if (file.size > this.maximumFileSize) {
      this.selectedFile = null;
      this.errorMessage =
        'The CV file must not exceed 5 MB.';
      input.value = '';
      return;
    }

    this.selectedFile = file;
  }

  // Uploads or replaces the selected CV.
  uploadCv(): void {
    if (!this.selectedFile) {
      this.errorMessage = 'Please choose a CV file.';
      return;
    }

    this.uploading = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.cvService
      .upload(this.userId, this.selectedFile)
      .subscribe({
        next: (cv) => {
          this.cv = cv;
          this.selectedFile = null;
          this.uploading = false;
          this.successMessage =
            'CV uploaded successfully.';
        },

        error: (error: HttpErrorResponse) => {
          this.uploading = false;
          this.errorMessage =
            this.getErrorMessage(
              error,
              'Unable to upload the CV.',
            );
        },
      });
  }

  // Downloads the current CV.
  downloadCv(): void {
    if (!this.cv) {
      return;
    }

    this.errorMessage = '';
    this.successMessage = '';

    this.cvService.download(this.userId).subscribe({
      next: (fileBlob) => {
        // Creates a temporary browser download URL.
        const downloadUrl =
          URL.createObjectURL(fileBlob);

        const link = document.createElement('a');

        link.href = downloadUrl;
        link.download = this.cv?.fileName ?? 'CV';
        link.click();

        // Removes the temporary URL from memory.
        URL.revokeObjectURL(downloadUrl);
      },

      error: (error: HttpErrorResponse) => {
        this.errorMessage =
          this.getErrorMessage(
            error,
            'Unable to download the CV.',
          );
      },
    });
  }

  // Deletes the current CV.
  deleteCv(): void {
    if (!this.cv) {
      return;
    }

    const confirmed = window.confirm(
      'Are you sure you want to delete this CV?',
    );

    if (!confirmed) {
      return;
    }

    this.deleting = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.cvService.delete(this.userId).subscribe({
      next: () => {
        this.cv = null;
        this.selectedFile = null;
        this.deleting = false;
        this.successMessage =
          'CV deleted successfully.';
      },

      error: (error: HttpErrorResponse) => {
        this.deleting = false;
        this.errorMessage =
          this.getErrorMessage(
            error,
            'Unable to delete the CV.',
          );
      },
    });
  }

  // Converts bytes into a readable file size.
  formatFileSize(bytes: number): string {
    if (bytes < 1024) {
      return `${bytes} B`;
    }

    if (bytes < 1024 * 1024) {
      return `${(bytes / 1024).toFixed(1)} KB`;
    }

    return `${(bytes / (1024 * 1024)).toFixed(1)} MB`;
  }

  // Returns a suitable API error message.
  private getErrorMessage(
    error: HttpErrorResponse,
    defaultMessage: string,
  ): string {
    if (error.status === 0) {
      return 'Cannot connect to the backend server.';
    }

    if (error.status === 400) {
      return (
        error.error?.message ??
        'The selected CV is invalid.'
      );
    }

    if (error.status === 404) {
      return 'CV was not found.';
    }

    return defaultMessage;
  }
}