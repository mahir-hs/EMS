import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../service/auth.service';
import { HttpRequest } from '@angular/common/http';
import { catchError, switchMap, throwError } from 'rxjs';
import { Router } from '@angular/router';

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const token = authService.getToken();

  if (req.url.includes('/refresh')) {
    return next(req);
  }
  const modifiedReq = token
    ? req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
        },
      })
    : req;

  console.log('Request sent:', modifiedReq);

  return next(modifiedReq).pipe(
    catchError((error) => {
      console.log('Interceptor caught an error:', error);
      if (token && error.status === 401) {
        console.warn('Token expired. Attempting to refresh...');
        return authService.refreshToken().pipe(
          switchMap((newToken) => {
            console.log('Token refreshed successfully:', newToken);
            const retryReq = req.clone({
              setHeaders: {
                Authorization: `Bearer ${newToken}`,
              },
            });

            console.log('Retrying request with new token:', retryReq);
            return next(retryReq);
          }),
          catchError((refreshError) => {
            console.error('Token refresh failed. Logging out...', refreshError);

            // Logout and redirect to login page
            authService.logout();
            router.navigate(['/login']);
            return throwError(
              () => new Error('Session expired. Please log in again.')
            );
          })
        );
      }

      // If error is not 401 or token refresh fails, rethrow the error
      return throwError(() => error);
    })
  );
};
