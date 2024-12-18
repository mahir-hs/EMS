import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class AttendanceService {
  private apiUrl = 'http://localhost:5092/api/employee-attendance';
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

  getUserAttendance(id: number): Observable<any[]> {
    const headers = this.createAuthorizationHeader();
    return this.http.get<any[]>(`${this.apiUrl}/get?id=${id}`, { headers });
  }

  addAttendance(id: number, attendance: any): Observable<any> {
    const headers = this.createAuthorizationHeader();
    return this.http.post<any>(`${this.apiUrl}/add?id=${id}`, attendance, {
      headers,
    });
  }
  updateAttendance(id: number, attendance: any): Observable<any> {
    const headers = this.createAuthorizationHeader();
    return this.http.patch<any>(`${this.apiUrl}/update?id=${id}`, attendance, {
      headers,
    });
  }

  getAttendanceById(id: number): Observable<any> {
    const headers = this.createAuthorizationHeader();
    return this.http.get<any>(`${this.apiUrl}/get?id=${id}`, { headers });
  }

  getAttendanceByAttendanceId(id: number): Observable<any> {
    const headers = this.createAuthorizationHeader();
    return this.http.get<any>(`${this.apiUrl}/get-single?id=${id}`, {
      headers,
    });
  }
}

export interface Attendance {
  id: number;
  employeeId: number;
  checkInTime: Date;
  checkOutTime?: Date;
}
