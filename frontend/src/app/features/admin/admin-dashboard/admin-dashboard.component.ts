import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';

import {
  AdminUser,
} from '../../../core/models/admin.model';
import {
  UserRole,
} from '../../../core/models/auth.model';
import {
  AdminService,
} from '../../../core/services/admin.service';
import {
  AuthService,
} from '../../../core/services/auth.service';

@Component({
  selector: 'app-admin-dashboard',
  imports: [CommonModule],
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.css',
})
export class AdminDashboardComponent implements OnInit {
  private readonly adminService = inject(AdminService);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  readonly userName =
    this.authService.getUserName() ?? 'Admin';

  users: AdminUser[] = [];
  isLoading = true;
  errorMessage = '';
  updatingUserIds = new Set<string>();

  readonly roles: UserRole[] = [
    'JobSeeker',
    'Employer',
    'Admin',
  ];

  ngOnInit(): void {
    this.loadUsers();
  }

  get totalUsers(): number {
    return this.users.length;
  }

  get jobSeekerCount(): number {
    return this.users.filter(
      (user) => user.role === 'JobSeeker',
    ).length;
  }

  get employerCount(): number {
    return this.users.filter(
      (user) => user.role === 'Employer',
    ).length;
  }

  get activeUserCount(): number {
    return this.users.filter(
      (user) => user.isActive,
    ).length;
  }

  loadUsers(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.adminService.getUsers().subscribe({
      next: (users) => {
        this.users = users;
        this.isLoading = false;
      },
      error: (error: HttpErrorResponse) => {
        this.errorMessage =
          error.status === 403
            ? 'You do not have Admin permission.'
            : 'Unable to load users. Please try again.';

        this.isLoading = false;
      },
    });
  }

  changeRole(
    user: AdminUser,
    event: Event,
  ): void {
    const selectElement =
      event.target as HTMLSelectElement;

    const newRole =
      selectElement.value as UserRole;

    if (newRole === user.role) {
      return;
    }

    this.updatingUserIds.add(user.id);
    this.errorMessage = '';

    this.adminService
      .updateUserRole(user.id, { role: newRole })
      .subscribe({
        next: (updatedUser) => {
          this.replaceUser(updatedUser);
          this.updatingUserIds.delete(user.id);
        },
        error: () => {
          selectElement.value = user.role;
          this.errorMessage =
            `Unable to update ${user.fullName}'s role.`;
          this.updatingUserIds.delete(user.id);
        },
      });
  }

  toggleStatus(user: AdminUser): void {
    this.updatingUserIds.add(user.id);
    this.errorMessage = '';

    this.adminService
      .updateUserStatus(
        user.id,
        { isActive: !user.isActive },
      )
      .subscribe({
        next: (updatedUser) => {
          this.replaceUser(updatedUser);
          this.updatingUserIds.delete(user.id);
        },
        error: () => {
          this.errorMessage =
            `Unable to update ${user.fullName}'s status.`;
          this.updatingUserIds.delete(user.id);
        },
      });
  }

  isUpdating(userId: string): boolean {
    return this.updatingUserIds.has(userId);
  }

  logout(): void {
    this.authService.logout();
    this.router.navigateByUrl('/login');
  }

  private replaceUser(updatedUser: AdminUser): void {
    this.users = this.users.map((user) =>
      user.id === updatedUser.id
        ? updatedUser
        : user,
    );
  }
}
