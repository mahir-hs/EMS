import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import {
  BehaviorSubject,
  catchError,
  map,
  Observable,
  tap,
  throwError,
} from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'http://localhost:5092/api/account';
  private isAuthenticated = new BehaviorSubject<boolean>(false);
  constructor(private http: HttpClient, private router: Router) {}

  getAuthStatus() {
    return this.isAuthenticated.asObservable();
  }

  register(user: any) {
    return this.http.post(`${this.apiUrl}/signup`, user);
  }

  login(credentials: any) {
    return this.http.post(`${this.apiUrl}/login`, credentials);
  }

  logout() {
    const accessToken = this.getToken();
    if (!accessToken) {
      console.warn('No access token found for logout');
      this.clearSession();
      this.router.navigate(['/login']);
      return;
    }

    this.http
      .post(`${this.apiUrl}/logout`, JSON.stringify(accessToken), {
        headers: { 'Content-Type': 'application/json' },
      })
      .subscribe({
        next: () => {
          this.clearSession();
          this.router.navigate(['/login']);
        },
        error: (err) => {
          console.error('Logout failed:', err);
          this.clearSession();
          this.router.navigate(['/login']);
        },
      });
  }

  clearSession() {
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
    this.isAuthenticated.next(false);
  }
  setToken(token: string) {
    localStorage.setItem('token', token);
    this.isAuthenticated.next(true);
  }

  setRefreshToken(token: string) {
    localStorage.setItem('refreshToken', token);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }
  redirectIfLoggedIn(): void {
    if (this.isLoggedIn()) {
      this.router.navigate(['/']);
    }
  }

  decodeToken(token: string): any {
    const payload = token.split('.')[1];
    return JSON.parse(atob(payload));
  }

  getEmailFromToken(token: string): string | null {
    const decoded = this.decodeToken(token);
    return decoded?.email || null;
  }

  refreshToken(): Observable<any> {
    const rT = localStorage.getItem('refreshToken');
    const aT = this.getToken();
    return this.http
      .post<{ accessToken: string; refreshToken: string }>(
        `${this.apiUrl}/refresh`,
        {
          accessToken: aT,
          refreshToken: rT,
        }
      )
      .pipe(
        tap((response) => {
          console.log('Refreshed token:', response.accessToken);
          this.setToken(response.accessToken);
        }),
        catchError((error) => {
          this.logout();
          return throwError(() => new Error('Session expired'));
        }),
        map((response) => response.accessToken)
      );
  }
}
