import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';

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
}
