import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AttendanceService {

  private apiUrl = 'http://localhost:5092/api/employee-attendance';
  private employeeUrl = 'http://localhost:5092/api/employee';
  constructor(private http: HttpClient) {}

  getAllEmployee(): Observable<any[]> {
    return this.http.get<any[]>(`${this.employeeUrl}/all`);
  }

  getAll(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/all`);
  }


  getAllAttendance(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/all`);
  }

  // Add a new attendance record
  addAttendance(attendance: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/add`, attendance);
  }

  // Update an attendance record (for example, check-out time)
  updateAttendance(id: number, attendance: any): Observable<any> {
    console.log(id);
    return this.http.patch<any>(`${this.apiUrl}/update?id=${id}`, attendance);
  }


  getAttendanceById(id: number):Observable<any>{

    return this.http.get<any>(`${this.apiUrl}/get?id=${id}`);
  }


}

export interface Attendance
{
  id:number,
  employeeId:number;
  checkInTime:Date;
  checkOutTime?:Date;

}
