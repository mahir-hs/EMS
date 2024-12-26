import { CommonModule } from '@angular/common';
import { AttendanceService } from './../../../service/attendance.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import * as XLSX from 'xlsx';
import { ngxCsv } from 'ngx-csv';
import { DownloadService } from '../../../service/download.service';

@Component({
  selector: 'app-attendance-user',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './attendance-user.component.html',
  styleUrl: './attendance-user.component.css',
})
export class AttendanceUserComponent implements OnInit {
  employeeId: number = 0;
  attendance: any[] = [];
  constructor(
    private route: ActivatedRoute,
    private attendanceService: AttendanceService,
    private downloadService: DownloadService,
    private router: Router
  ) {}
  ngOnInit(): void {
    this.employeeId = +this.route.snapshot.paramMap.get('id')!;
    this.getAttendanceDetails();
  }

  getAttendanceDetails(): void {
    this.attendanceService.getUserAttendance(this.employeeId).subscribe({
      next: (data) => {
        this.attendance = data;
      },
      error: (err) => {
        console.error('Error fetching attendance details');
      },
    });
  }

  addAttendance(): void {
    const newAttendance = {
      checkInTime: new Date().toISOString().split('.')[0],
      checkOutTime: null,
      employeeId: this.employeeId,
    };

    this.attendanceService
      .addAttendance(this.employeeId, newAttendance)
      .subscribe({
        next: (response) => {
          console.log('Attendance added successfully:', response);
          this.getAttendanceDetails();
        },
        error: (err) => {
          console.error('Error adding attendance:', err);
        },
      });
  }

  exportToCSV(): void {
    this.downloadService
      .employeeAttendanceExportCsv(this.employeeId)
      .subscribe({
        next: (response) => {
          const blob = new Blob([response], { type: 'text/csv' });
          const url = window.URL.createObjectURL(blob);
          const a = document.createElement('a');
          a.href = url;
          a.download = 'EmployeeAttendanceList.csv';
          a.click();
          window.URL.revokeObjectURL(url);
        },
        error: (err) => {
          console.error('Failed to download CSV:', err);
        },
      });
  }

  exportToExcel(): void {
    this.downloadService
      .employeeAttendanceExportExcel(this.employeeId)
      .subscribe({
        next: (response) => {
          const blob = new Blob([response], {
            type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
          });
          const url = window.URL.createObjectURL(blob);
          const a = document.createElement('a');
          a.href = url;
          a.download = 'EmployeeAttendanceList.xlsx';
          a.click();
          window.URL.revokeObjectURL(url);
        },
        error: (err) => {
          console.error('Failed to download Excel:', err);
        },
      });
  }
}
