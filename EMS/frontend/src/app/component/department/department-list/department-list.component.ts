import { Employee } from './../../../service/employee.service';
import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import {
  Department,
  DepartmentService,
} from '../../../service/department.service';
import { AuthService } from '../../../service/auth.service';

@Component({
  selector: 'app-department-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './department-list.component.html',
  styleUrl: './department-list.component.css',
})
export class DepartmentListComponent implements OnInit {
  department: Department[] = [];
  loading: boolean = true;

  constructor(
    private departmentService: DepartmentService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.fetchDepartments();
  }
  isRole(role: string): boolean {
    return this.authService.getRole() === role;
  }
  fetchDepartments(): void {
    this.departmentService.getAll().subscribe({
      next: (data) => {
        this.department = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to fetch department', err);
        this.loading = false;
      },
    });
  }

  deleteDepartment(id: number): void {
    if (confirm('Are you sure you want to delete this department?')) {
      this.departmentService.delete(id).subscribe({
        next: () => {
          this.department = this.department.filter(
            (department) => department.id != id
          );
        },
        error: (error) => {
          console.error('Error deleting department: ', error);
        },
      });
    }
  }
}
