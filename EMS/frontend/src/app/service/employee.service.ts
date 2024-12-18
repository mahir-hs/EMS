import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class EmployeeService {
  private apiUrl = 'http://localhost:5092/api/employee';

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

  add(employee: any): Observable<any> {
    const headers = this.createAuthorizationHeader();
    return this.http.post<any>(`${this.apiUrl}/add`, employee, { headers });
  }

  getById(id: number): Observable<any> {
    const headers = this.createAuthorizationHeader();
    return this.http.get<any>(`${this.apiUrl}/get?id=${id}`, { headers });
  }

  update(id: number, employee: any): Observable<any> {
    const headers = this.createAuthorizationHeader();
    return this.http.patch<any>(`${this.apiUrl}/update?id=${id}`, employee, {
      headers,
    });
  }

  delete(id: number): Observable<any> {
    const headers = this.createAuthorizationHeader();
    return this.http.delete<any>(`${this.apiUrl}/delete?id=${id}`, { headers });
  }
}

export interface Employee {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  address: string;
  dateOfBirth: string;
  departmentId?: number;
  designationId?: number;
}
