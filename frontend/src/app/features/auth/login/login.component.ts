import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import {
  ActivatedRoute,
  Router,
  RouterLink,
} from '@angular/router';
import { finalize } from 'rxjs';

import { UserRole } from '../../../core/models/auth.model';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink,
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  private readonly formBuilder = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  isSubmitting = false;
  errorMessage = '';
  showPassword = false;

  darkMode =
    localStorage.getItem('authDarkMode') === 'true';

  readonly loginForm = this.formBuilder.nonNullable.group({
    email: [
      '',
      [
        Validators.required,
        Validators.email,
        Validators.maxLength(200),
      ],
    ],
    password: [
      '',
      [
        Validators.required,
      ],
    ],
  });

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
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

    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;

    this.authService
      .login(this.loginForm.getRawValue())
      .pipe(
        finalize(() => {
          this.isSubmitting = false;
        }),
      )
      .subscribe({
        next: (response) => {
          this.redirectAfterLogin(response.role);
        },
        error: (error: HttpErrorResponse) => {
          this.errorMessage =
            error.error?.message ??
            'Login failed. Please try again.';
        },
      });
  }

  private redirectAfterLogin(role: UserRole): void {
    const returnUrl =
      this.route.snapshot.queryParamMap.get(
        'returnUrl',
      );

    if (returnUrl?.startsWith('/')) {
      this.router.navigateByUrl(returnUrl);
      return;
    }

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