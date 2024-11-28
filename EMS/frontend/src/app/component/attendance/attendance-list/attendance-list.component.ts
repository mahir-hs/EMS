import { Component, OnInit } from '@angular/core';
import { AttendanceService } from '../../../service/attendance.service'; // Import the attendance service
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-attendance-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './attendance-list.component.html',
  styleUrls: ['./attendance-list.component.css'],
})
export class AttendanceListComponent implements OnInit {
  attendanceRecords: any[] = [];
  filteredAttendance: any[] = [];
  loading: boolean = true;
  searchText: string = '';
  constructor(private attendanceService: AttendanceService) {}

  ngOnInit(): void {
    this.fetchAttendance();
  }

  fetchAttendance(): void {
    this.attendanceService.getAll().subscribe({
      next: (data) => {
        this.attendanceRecords = data;
        this.filteredAttendance = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error fetching attendance records:', err);
        this.loading = false;
      },
    });
  }

  filterAttendance(): void {
    if (this.searchText) {
      this.filteredAttendance = this.attendanceRecords.filter((record) =>
        record.employeeId.toString().includes(this.searchText)
      );
    } else {
      this.filteredAttendance = this.attendanceRecords;
    }
  }
}
