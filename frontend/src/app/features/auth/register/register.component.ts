import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  ReactiveFormsModule,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import {
  Router,
  RouterLink,
} from '@angular/router';
import { finalize } from 'rxjs';

import { UserRole } from '../../../core/models/auth.model';
import { AuthService } from '../../../core/services/auth.service';

const passwordMatchValidator: ValidatorFn = (
  control: AbstractControl,
): ValidationErrors | null => {
  const password = control.get('password')?.value;
  const confirmPassword =
    control.get('confirmPassword')?.value;

  return password === confirmPassword
    ? null
    : { passwordMismatch: true };
};

@Component({
  selector: 'app-register',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink,
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  private readonly formBuilder = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  isSubmitting = false;
  errorMessage = '';
  showPassword = false;
  showConfirmPassword = false;

  darkMode =
    localStorage.getItem('authDarkMode') === 'true';

  readonly registerForm = this.formBuilder.nonNullable.group(
    {
      fullName: [
        '',
        [
          Validators.required,
          Validators.maxLength(150),
        ],
      ],
      email: [
        '',
        [
          Validators.required,
          Validators.email,
          Validators.maxLength(200),
        ],
      ],
      role:
        this.formBuilder.nonNullable.control<UserRole>(
          'JobSeeker',
          Validators.required,
        ),
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(8),
          Validators.maxLength(100),
        ],
      ],
      confirmPassword: [
        '',
        [
          Validators.required,
        ],
      ],
    },
    {
      validators: passwordMatchValidator,
    },
  );

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  toggleConfirmPasswordVisibility(): void {
    this.showConfirmPassword =
      !this.showConfirmPassword;
  }

  toggleDarkMode(): void {
    this.darkMode = !this.darkMode;

    localStorage.setItem(
      'authDarkMode',
      String(this.darkMode),
    );
  }

  onSubmit(): void {
    this.errorMessage = '';

    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;

    this.authService
      .register(this.registerForm.getRawValue())
      .pipe(
        finalize(() => {
          this.isSubmitting = false;
        }),
      )
      .subscribe({
        next: (response) => {
          this.redirectAfterRegister(response.role);
        },
        error: (error: HttpErrorResponse) => {
          this.errorMessage =
            error.error?.message ??
            'Registration failed. Please try again.';
        },
      });
  }

  private redirectAfterRegister(role: UserRole): void {
    const destinationByRole: Record<UserRole, string> = {
      JobSeeker: '/seeker/profile',
      Employer: '/employer/profile',
      Admin: '/admin/dashboard',
    };

    this.router.navigateByUrl(
      destinationByRole[role],
    );
  }
}