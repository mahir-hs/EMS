import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class DownloadService {
  private apiUrl = 'http://localhost:5092/api/Download';

  constructor(private http: HttpClient) {}
  employeeExportCsv(): Observable<any> {
    return this.http.get(`${this.apiUrl}/export-to-csv`, {
      responseType: 'blob',
    });
  }

  employeeExportExcel(): Observable<any> {
    return this.http.get(`${this.apiUrl}/export-to-excel`, {
      responseType: 'blob',
    });
  }

  employeeAttendanceExportCsv(id: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/attendance-export-to-csv?id=${id}`, {
      responseType: 'blob',
    });
  }

  employeeAttendanceExportExcel(id: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/attendance-export-to-excel?id=${id}`, {
      responseType: 'blob',
    });
  }
}
