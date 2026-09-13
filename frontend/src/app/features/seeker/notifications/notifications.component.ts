import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import {
  Router,
  RouterLink,
} from '@angular/router';

import { Notification } from '../../../core/models/notification.model';
import { AuthService } from '../../../core/services/auth.service';
import { NotificationService } from '../../../core/services/notification.service';

@Component({
  selector: 'app-notifications',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
  ],
  templateUrl: './notifications.component.html',
  styleUrl: './notifications.component.css',
})
export class NotificationsComponent implements OnInit {
  private readonly notificationService =
    inject(NotificationService);

  private readonly authService =
    inject(AuthService);

  private readonly router =
    inject(Router);

  readonly userName =
    this.authService.getUserName() ?? 'User';

  notifications: Notification[] = [];

  loading = false;
  errorMessage = '';
  darkMode = false;

  ngOnInit(): void {
    this.loadNotifications();
  }

  loadNotifications(): void {
    this.loading = true;
    this.errorMessage = '';

    this.notificationService
      .getMyNotifications()
      .subscribe({
        next: (notifications) => {
          this.notifications = notifications;
          this.loading = false;
        },
        error: () => {
          this.errorMessage =
            'Unable to load notifications.';
          this.loading = false;
        },
      });
  }

  logout(): void {
    this.authService.logout();
    this.router.navigateByUrl('/login');
  }

  toggleDarkMode(): void {
    this.darkMode = !this.darkMode;
  }
}