import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import {
  Component,
  OnInit,
  inject,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
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
  imports: [
    CommonModule,
    FormsModule,
  ],
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

  darkMode = false;

  searchTerm = '';
  roleFilter = 'All';
  statusFilter = 'All';

  currentPage = 1;
  readonly pageSize = 5;

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

  get adminCount(): number {
    return this.users.filter(
      (user) => user.role === 'Admin',
    ).length;
  }

  get activeUserCount(): number {
    return this.users.filter(
      (user) => user.isActive,
    ).length;
  }

  get blockedUserCount(): number {
    return this.users.filter(
      (user) => !user.isActive,
    ).length;
  }

  get jobSeekerPercentage(): number {
    return this.getPercentage(
      this.jobSeekerCount,
    );
  }

  get employerPercentage(): number {
    return this.getPercentage(
      this.employerCount,
    );
  }

  get adminPercentage(): number {
    return this.getPercentage(
      this.adminCount,
    );
  }

  get activeUserPercentage(): number {
    return this.getPercentage(
      this.activeUserCount,
    );
  }

  get filteredUsers(): AdminUser[] {
    const search =
      this.searchTerm.trim().toLowerCase();

    return this.users.filter((user) => {
      const matchesSearch =
        !search ||
        user.fullName.toLowerCase().includes(search) ||
        user.email.toLowerCase().includes(search);

      const matchesRole =
        this.roleFilter === 'All' ||
        user.role === this.roleFilter;

      const matchesStatus =
        this.statusFilter === 'All' ||
        (
          this.statusFilter === 'Active' &&
          user.isActive
        ) ||
        (
          this.statusFilter === 'Blocked' &&
          !user.isActive
        );

      return (
        matchesSearch &&
        matchesRole &&
        matchesStatus
      );
    });
  }

  get totalPages(): number {
    return Math.max(
      1,
      Math.ceil(
        this.filteredUsers.length /
        this.pageSize,
      ),
    );
  }

  get paginatedUsers(): AdminUser[] {
    const start =
      (this.currentPage - 1) *
      this.pageSize;

    return this.filteredUsers.slice(
      start,
      start + this.pageSize,
    );
  }

  loadUsers(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.adminService.getUsers().subscribe({
      next: (users) => {
        this.users = users;
        this.currentPage = 1;
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

  applyFilters(): void {
    this.currentPage = 1;
  }

  clearFilters(): void {
    this.searchTerm = '';
    this.roleFilter = 'All';
    this.statusFilter = 'All';
    this.currentPage = 1;
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
    }
  }

  goToPage(page: number): void {
    if (
      page >= 1 &&
      page <= this.totalPages
    ) {
      this.currentPage = page;
    }
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
      .updateUserRole(
        user.id,
        { role: newRole },
      )
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
        {
          isActive: !user.isActive,
        },
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

  goBack(): void {
    window.history.back();
  }

  toggleDarkMode(): void {
    this.darkMode = !this.darkMode;
  }

  logout(): void {
    this.authService.logout();
    this.router.navigateByUrl('/login');
  }

  private replaceUser(
    updatedUser: AdminUser,
  ): void {
    this.users = this.users.map(
      (user) =>
        user.id === updatedUser.id
          ? updatedUser
          : user,
    );
  }

  private getPercentage(
    count: number,
  ): number {
    if (this.totalUsers === 0) {
      return 0;
    }

    return Math.round(
      (count / this.totalUsers) * 100,
    );
  }
}