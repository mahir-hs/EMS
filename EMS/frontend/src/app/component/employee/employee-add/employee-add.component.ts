import { DesignationService } from './../../../service/designation.service';
import { DepartmentService } from './../../../service/department.service';
import { EmployeeService } from './../../../service/employee.service';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgSelectModule } from '@ng-select/ng-select';
@Component({
  selector: 'app-employee-add',
  standalone: true,
  imports: [FormsModule, CommonModule, NgSelectModule],
  templateUrl: './employee-add.component.html',
  styleUrls: ['./employee-add.component.css'],
})
export class EmployeeAddComponent {
  employee = {
    firstName: '',
    lastName: '',
    email: '',
    phone: '',
    address: '',
    dateOfBirth: '',
    departmentId: null,
    designationId: null,
  };

  designation: any[] = [];
  department: any[] = [];

  constructor(
    private employeeService: EmployeeService,
    private departmentService: DepartmentService,
    private designationService: DesignationService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.getDepartment();
    this.getDesignation();
  }

  getDepartment(): void {
    this.departmentService.getAll().subscribe({
      next: (data) => {
        this.department = data;
        // console.log('Departments:', this.department);
      },
      error: (err) => {
        console.log('Error fetching department: ', err);
      },
    });
  }

  getDesignation(): void {
    this.designationService.getAll().subscribe({
      next: (data) => {
        this.designation = data;
        // console.log('Designation:', this.designation);
      },
      error: (err) => {
        console.log('Error fetching designation: ', err);
      },
    });
  }

  addEmployee(): void {
    this.employeeService.add(this.employee).subscribe({
      next: () => {
        this.router.navigate(['']);
      },
      error: (err) => {
        console.error('Error adding employee', err);
      },
    });
  }
}
