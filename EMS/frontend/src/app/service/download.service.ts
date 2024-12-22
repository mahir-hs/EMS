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
}
