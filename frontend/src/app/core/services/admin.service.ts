import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import {
  AdminUser,
  UpdateUserRoleRequest,
  UpdateUserStatusRequest,
} from '../models/admin.model';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl =
    `${environment.apiUrl}/Admin/users`;

  getUsers(): Observable<AdminUser[]> {
    return this.http.get<AdminUser[]>(this.apiUrl);
  }

  updateUserRole(
    userId: string,
    request: UpdateUserRoleRequest,
  ): Observable<AdminUser> {
    return this.http.put<AdminUser>(
      `${this.apiUrl}/${userId}/role`,
      request,
    );
  }

  updateUserStatus(
    userId: string,
    request: UpdateUserStatusRequest,
  ): Observable<AdminUser> {
    return this.http.put<AdminUser>(
      `${this.apiUrl}/${userId}/status`,
      request,
    );
  }
}