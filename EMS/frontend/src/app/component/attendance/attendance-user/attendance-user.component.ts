import { CommonModule } from '@angular/common';
import { AttendanceService } from './../../../service/attendance.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import * as XLSX from 'xlsx';
import { ngxCsv } from 'ngx-csv';

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
    const csvData = this.attendance.map((att) => ({
      ID: att.id,
      CheckInTime: att.checkInTime,
      CheckOutTime: att.checkOutTime,
    }));

    const options = {
      headers: ['ID', 'CheckInTime', 'CheckOutTime'],
    };
    new ngxCsv(csvData, 'EmployeeAttendance', options);
  }

  exportToExcel(): void {
    const ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(
      this.attendance.map((att) => ({
        ID: att.id,
        CheckInTime: att.checkInTime,
        CheckOutTime: att.checkOutTime,
      }))
    );
    const wb: XLSX.WorkBook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'EmployeeAttendance');

    XLSX.writeFile(wb, 'employee-attendance.xlsx');
  }
}
