import { Component, OnInit } from '@angular/core';
import { EmployeeService,Employee } from '../../../service/employee.service';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
@Component({
  selector: 'app-employee-list',
  standalone: true,
  imports: [CommonModule,RouterLink],
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.css']
})


export class EmployeeListComponent implements OnInit {
  employees: Employee[] = [];
  loading: boolean = true;

  constructor(private employeeService: EmployeeService) {}

  ngOnInit(): void {
    this.fetchEmployees();
  }

  fetchEmployees(): void {
    this.employeeService.getAll().subscribe({
      next: (data) => {
        this.employees = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to fetch employees:', err);
        this.loading = false;
      },
    });
  }

  deleteEmployee(id: number): void {
    if (confirm('Are you sure you want to delete this employee?')) {
      this.employeeService.delete(id).subscribe({
        next: () => {
          this.employees = this.employees.filter(employee => employee.id !== id);
        },
        error: (error) => {
          console.error('Error deleting employee:', error);
        }
      });
    }
  }
}
