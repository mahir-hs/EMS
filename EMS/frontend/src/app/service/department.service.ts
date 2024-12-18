import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class DepartmentService {
  private apiUrl = 'http://localhost:5092/api/department';

  constructor(private http: HttpClient, private authService: AuthService) {}

  private createAuthorizationHeader(): HttpHeaders {
    const token = this.authService.getToken();
    if (!token) {
      console.error('No token found');
      return new HttpHeaders();
    }
    return new HttpHeaders().set('Authorization', `Bearer ${token}`);
  }

  getAll(): Observable<any[]> {
    const headers = this.createAuthorizationHeader();
    return this.http.get<any[]>(`${this.apiUrl}/all`, { headers });
  }

  add(department: any): Observable<any> {
    const headers = this.createAuthorizationHeader();
    return this.http.post<any>(`${this.apiUrl}/add`, department, { headers });
  }

  getById(id: number): Observable<any> {
    const headers = this.createAuthorizationHeader();
    return this.http.get<any>(`${this.apiUrl}/get?id=${id}`, { headers });
  }

  update(id: number, department: any): Observable<any> {
    const headers = this.createAuthorizationHeader();
    return this.http.patch<any>(`${this.apiUrl}/update?id=${id}`, department, {
      headers,
    });
  }

  delete(id: number): Observable<any> {
    const headers = this.createAuthorizationHeader();
    return this.http.delete<any>(`${this.apiUrl}/delete?Id=${id}`, { headers });
  }
}

export interface Department {
  id: number;
  dept: string;
}
