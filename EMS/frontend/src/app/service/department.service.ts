import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {

  private apiUrl = 'http://localhost:5092/api/department';

  constructor(private http:HttpClient) { }

  getAll(): Observable<any[]>{
    return this.http.get<any[]>(`${this.apiUrl}/all`);
  }

  add(department: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/add`, department);
  }

  getById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/get?id=${id}`);
  }

  update(id: number, department: any): Observable<any> {
    return this.http.patch<any>(`${this.apiUrl}/update?id=${id}`, department);
  }

  delete(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/delete?Id=${id}`);
  }

}

export interface Department{
  id: number;
  dept:string;
}
