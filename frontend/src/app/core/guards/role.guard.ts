import { inject } from '@angular/core';
import {
  CanActivateFn,
  Router,
} from '@angular/router';

import { UserRole } from '../models/auth.model';
import { AuthService } from '../services/auth.service';

export const roleGuard: CanActivateFn = (
  route,
  state,
) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (!authService.isAuthenticated()) {
    return router.createUrlTree(
      ['/login'],
      {
        queryParams: {
          returnUrl: state.url,
        },
      },
    );
  }

  const allowedRoles =
    route.data['roles'] as UserRole[] | undefined;

  const currentRole = authService.getRole();

  if (
    currentRole &&
    allowedRoles?.includes(currentRole)
  ) {
    return true;
  }

  return router.createUrlTree(['/unauthorized']);
};