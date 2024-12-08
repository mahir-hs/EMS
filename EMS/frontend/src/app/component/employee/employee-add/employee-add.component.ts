import { DesignationService } from './../../../service/designation.service';
import { DepartmentService } from './../../../service/department.service';
import { EmployeeService } from './../../../service/employee.service';
import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-employee-add',
  standalone: true,
  imports: [FormsModule, RouterModule, ReactiveFormsModule, CommonModule],
  templateUrl: './employee-add.component.html',
  styleUrls: ['./employee-add.component.css'],
})
export class EmployeeAddComponent implements OnInit {
  designation: any[] = [];
  department: any[] = [];
  employeeForm!: FormGroup;

  constructor(
    private employeeService: EmployeeService,
    private departmentService: DepartmentService,
    private designationService: DesignationService,
    private router: Router,
    private fb: FormBuilder
  ) {}
  ngOnInit(): void {
    this.initForm();
    this.getDepartment();
    this.getDesignation();
  }
  initForm(): void {
    this.employeeForm = this.fb.group({
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required]],
      address: ['', [Validators.required]],
      dateOfBirth: ['', [Validators.required]],
      departmentId: ['', [Validators.required]],
      designationId: ['', [Validators.required]],
    });
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

  addEmployee(): void {
    console.log(this.employeeForm.value);
    this.employeeService.add(this.employeeForm.value).subscribe({
      next: () => {
        this.router.navigate(['']);
      },
      error: (err) => {
        console.error('Error adding employee', err);
      },
    });
  }
}
