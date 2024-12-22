import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class AttendanceService {
  private apiUrl = 'http://localhost:5092/api/employee-attendance';
  constructor(private http: HttpClient) {}

  getAll(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/all`);
  }

  getUserAttendance(id: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/get?id=${id}`);
  }

  addAttendance(id: number, attendance: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/add?id=${id}`, attendance);
  }
  updateAttendance(id: number, attendance: any): Observable<any> {
    return this.http.patch<any>(`${this.apiUrl}/update?id=${id}`, attendance);
  }

  getAttendanceById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/get?id=${id}`);
  }

  getAttendanceByAttendanceId(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/get-single?id=${id}`);
  }
}

export interface Attendance {
  id: number;
  employeeId: number;
  checkInTime: Date;
  checkOutTime?: Date;
}
