import { CommonModule } from '@angular/common';
import { AttendanceService } from './../../../service/attendance.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';

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
    this.router.navigate([`/attendance-add/${this.employeeId}`]);
  }
}
