import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { AttendanceService } from '../../../service/attendance.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-attendance-update',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule],
  templateUrl: './attendance-update.component.html',
  styleUrl: './attendance-update.component.css',
})
export class AttendanceUpdateComponent implements OnInit {
  attendanceId: number = 0;
  attendance = {
    checkInTime: new Date(),
    checkOutTime: new Date(),
  };

  constructor(
    private route: ActivatedRoute,
    private attendanceService: AttendanceService,
    private location: Location,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.attendanceId = +this.route.snapshot.paramMap.get('id')!;
    console.log(this.attendanceId);
    this.getAttendanceDetails();
  }

  getAttendanceDetails(): void {
    this.attendanceService
      .getAttendanceByAttendanceId(this.attendanceId)
      .subscribe({
        next: (data) => {
          this.attendance = data;
        },
        error: (err) => {
          console.error('Error fetching attendance details:', err);
        },
      });
  }

  updateAttendance(): void {
    this.attendance = {
      checkInTime:
        this.attendance.checkInTime || new Date().toISOString().slice(0, 16),
      checkOutTime:
        this.attendance.checkOutTime || new Date().toISOString().slice(0, 16),
    };

    this.attendanceService
      .updateAttendance(this.attendanceId, this.attendance)
      .subscribe({
        next: () => {
          if (window.history.length > 1) {
            this.location.back();
          } else {
            this.router.navigate(['/attendance-list']);
          }
        },
        error: (err) => {
          console.error('Error updating attendance:', err);
        },
      });
  }
  cancelUpdate(): void {
    if (window.history.length > 1) {
      this.location.back();
    } else {
      this.router.navigate(['/attendance-list']);
    }
  }
}
