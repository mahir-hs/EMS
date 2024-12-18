import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root',
})
export class DownloadService {
  private apiUrl = 'http://localhost:5092/api/Download';

  constructor(private http: HttpClient, private authService: AuthService) {}
  private createAuthorizationHeader(): HttpHeaders {
    const token = this.authService.getToken();
    if (!token) {
      console.error('No token found');
      return new HttpHeaders();
    }
    return new HttpHeaders().set('Authorization', `Bearer ${token}`);
  }
  employeeExportCsv(): Observable<any> {
    const headers = this.createAuthorizationHeader();
    return this.http.get(`${this.apiUrl}/export-to-csv`, {
      headers,
      responseType: 'blob',
    });
  }

  employeeExportExcel(): Observable<any> {
    const headers = this.createAuthorizationHeader();
    return this.http.get(`${this.apiUrl}/export-to-excel`, {
      headers,
      responseType: 'blob',
    });
  }
}
