import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class EmployeeService {
  private apiUrl = 'http://localhost:5092/api/employee';

  constructor(private http: HttpClient) {}

  getAll(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/all`);
  }



  add(employee: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/add`, employee);
  }

  getById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/get?id=${id}`);
  }

  update(id: number, employee: any): Observable<any> {
    return this.http.patch<any>(`${this.apiUrl}/update?id=${id}`, employee);
  }

  delete(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/delete?Id=${id}`);
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
