import { inject } from '@angular/core';
import {
  HttpInterceptorFn,
} from '@angular/common/http';

import { environment } from '../../../environments/environment';
import { AuthService } from '../services/auth.service';

export const authInterceptor: HttpInterceptorFn = (
  request,
  next,
) => {
  const authService = inject(AuthService);
  const token = authService.getToken();

  const isBackendRequest =
    request.url.startsWith(environment.apiUrl);

  if (!token || !isBackendRequest) {
    return next(request);
  }

  const authenticatedRequest = request.clone({
    setHeaders: {
      Authorization: `Bearer ${token}`,
    },
  });

  return next(authenticatedRequest);
};