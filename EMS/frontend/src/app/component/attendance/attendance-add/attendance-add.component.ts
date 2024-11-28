import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AttendanceService } from '../../../service/attendance.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule, Location } from '@angular/common';
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
    checkInTime: new Date(),
  };

  employees: any[] = [];
  employeeId: number = 0;

  constructor(
    private location: Location,
    private attendanceService: AttendanceService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.getEmployees();
  }

  getEmployees(): void {
    this.employeeId = +this.route.snapshot.paramMap.get('id')!;
  }

  addAttendance(): void {
    if (!this.attendance.checkInTime) {
      console.error('Please provide both check-in time');
      return;
    }

    const attendanceData = {
      checkInTime: this.attendance.checkInTime,
    };

    this.attendanceService
      .addAttendance(this.employeeId, attendanceData)
      .subscribe({
        next: (response) => {
          if (window.history.length > 1) this.location.back();
          else this.router.navigate(['/']);
        },
        error: (err) => {
          console.error('Error adding attendance:', err);
        },
      });
  }
  cancelAttendance(): void {
    if (window.history.length > 1) this.location.back();
    else this.router.navigate(['/']);
  }
}
