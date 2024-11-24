import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { Attendance, AttendanceService } from '../../../service/attendance.service';

@Component({
  selector: 'app-attendance-update',
  standalone: true,
  imports: [FormsModule,CommonModule,RouterModule],
  templateUrl: './attendance-update.component.html',
  styleUrl: './attendance-update.component.css'
})
export class AttendanceUpdateComponent implements OnInit {
  attendanceId: number = 0;
  attendance = {
    checkInTime: new Date(),
    checkOutTime: new Date()
  };

  attendance2 = {
    checkInTime: new Date(),
    checkOutTime: new Date()
  };

  constructor(
    private route: ActivatedRoute,
    private attendanceService: AttendanceService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.attendanceId = +this.route.snapshot.paramMap.get('id')!;
    this.getAttendanceDetails();
  }

  getAttendanceDetails(): void {
    this.attendanceService.getAttendanceById(this.attendanceId).subscribe({
      next: (data) => {
        this.attendance = data;
      },
      error: (err) => {
        console.error('Error fetching attendance details:', err);
      }
    });
  }



  updateAttendance(): void {
    this.attendance = {
      checkInTime: this.attendance.checkInTime || new Date().toISOString().slice(0, 16),
      checkOutTime: this.attendance.checkOutTime || new Date().toISOString().slice(0, 16)
    };

    console.log('Preparing to update attendance:', this.attendanceId);
    this.attendanceService.updateAttendance(this.attendanceId, this.attendance).subscribe({
      next: () => {
        this.router.navigate(['/attendance-list']);
      },
      error: (err) => {
        console.error('Error updating attendance:', err);
      }
    });
  }
  cancelUpdate(): void {
    this.router.navigate(['/attendance-list']);
  }
}
