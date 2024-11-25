import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AttendanceService } from '../../../service/attendance.service';
import { Router } from '@angular/router';
import { EmployeeService } from '../../../service/employee.service';
import { CommonModule } from '@angular/common';
import { NgSelectModule } from '@ng-select/ng-select';

@Component({
  selector: 'app-attendance-add',
  standalone: true,
  imports: [FormsModule, CommonModule, NgSelectModule],
  templateUrl: './attendance-add.component.html',
  styleUrl: './attendance-add.component.css',
})
export class AttendanceAddComponent implements OnInit {
  attendance = {
    employeeId: 0,
    checkInTime: new Date(),
  };

  employees: any[] = [];
  selectedEmployeeId: number = 0;

  constructor(
    private attendanceService: AttendanceService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.getEmployees();
  }

  getEmployees(): void {
    this.attendanceService.getAllEmployee().subscribe({
      next: (data) => {
        this.employees = data;
        console.log(this.employees);
      },
      error: (err) => {
        console.error('Error fetching employees:', err);
      },
    });
  }

  onEmployeeChange(): void {
    this.attendance.employeeId = this.selectedEmployeeId;
  }

  addAttendance(): void {
    if (!this.attendance.checkInTime || !this.attendance.employeeId) {
      console.error('Please provide both check-in time and EmployeeId.');
      return;
    }

    if (this.selectedEmployeeId === 0) {
      console.error('Please select an employee.');
      return;
    }

    console.log(this.selectedEmployeeId);

    const attendanceData = {
      employeeId: this.attendance.employeeId,
      checkInTime: this.attendance.checkInTime,
    };

    this.attendanceService.addAttendance(attendanceData).subscribe({
      next: (response) => {
        console.log('Attendance added successfully:', response);
        this.router.navigate(['/attendance-list']);
      },
      error: (err) => {
        console.error('Error adding attendance:', err);
      },
    });
  }
}
