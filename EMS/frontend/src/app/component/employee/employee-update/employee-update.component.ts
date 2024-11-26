import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EmployeeService } from '../../../service/employee.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { DepartmentService } from '../../../service/department.service';
import { DesignationService } from '../../../service/designation.service';

@Component({
  selector: 'app-employee-update',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './employee-update.component.html',
  styleUrls: ['./employee-update.component.css'],
})
export class EmployeeUpdateComponent implements OnInit {
  employeeId: number = 0;

  employee = {
    firstName: '',
    lastName: '',
    email: '',
    phone: '',
    address: '',
    dateOfBirth: new Date(),
    departmentId: null,
    designationId: null,
  };
  designation: any[] = [];
  department: any[] = [];

  constructor(
    private route: ActivatedRoute,
    private employeeService: EmployeeService,
    private departmentService: DepartmentService,
    private designationService: DesignationService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.employeeId = +this.route.snapshot.paramMap.get('id')!;
    this.getEmployeeDetails();
    this.getDepartment();
    this.getDesignation();
  }

  getDepartment(): void {
    this.departmentService.getAll().subscribe({
      next: (data) => {
        this.department = data.map((dept: any) => ({
          id: dept.id,
          name: dept.dept,
        }));
      },
      error: (err) => {
        console.log('Error fetching department: ', err);
      },
    });
  }

  getDesignation(): void {
    this.designationService.getAll().subscribe({
      next: (data) => {
        this.designation = data.map((desig: any) => ({
          id: desig.id,
          name: desig.role,
        }));
      },
      error: (err) => {
        console.log('Error fetching designation: ', err);
      },
    });
  }

  getEmployeeDetails(): void {
    this.employeeService.getById(this.employeeId).subscribe({
      next: (data) => {
        this.employee = {
          ...data,
          dateOfBirth: data.dateOfBirth ? data.dateOfBirth.split('T')[0] : '',
        };
        console.log(this.employee);
      },
      error: (error) => {
        console.error('Error fetching employee details:', error);
      },
    });
  }

  updateEmployee(): void {
    this.employeeService.update(this.employeeId, this.employee).subscribe({
      next: () => {
        this.router.navigate(['']);
      },
      error: (error) => {
        console.error('Error updating employee:', error);
      },
    });
  }
}
