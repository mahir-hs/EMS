import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../service/auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isLoggedIn()) {
    return true;
  }

  const expectedRole = route.data['role'];
  if (authService.isLoggedIn()) {
    const userRole = authService.getRole();
    if (userRole === 'Admin') {
      return true;
    }
    if (expectedRole && authService.isRole(expectedRole)) {
      return true;
    } else {
      // router.navigate(['/unauthorized']);
      router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
      return false;
    }
  }

  router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
  return false;
};
