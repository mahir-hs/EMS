import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EmployeeService } from '../../../service/employee.service';
import { CommonModule } from '@angular/common';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  Validators,
} from '@angular/forms';
import { DepartmentService } from '../../../service/department.service';
import { DesignationService } from '../../../service/designation.service';
import { ReactiveFormsModule } from '@angular/forms';
@Component({
  selector: 'app-employee-update',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './employee-update.component.html',
  styleUrls: ['./employee-update.component.css'],
})
export class EmployeeUpdateComponent implements OnInit {
  employeeId: number = 0;

  // employee = {
  //   firstName: '',
  //   lastName: '',
  //   email: '',
  //   phone: '',
  //   address: '',
  //   dateOfBirth: new Date(),
  //   departmentId: null,
  //   designationId: null,
  // };
  designation: any[] = [];
  department: any[] = [];
  employeeForm!: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private employeeService: EmployeeService,
    private departmentService: DepartmentService,
    private designationService: DesignationService,
    private router: Router,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.initForm();
    this.employeeId = +this.route.snapshot.paramMap.get('id')!;
    this.getEmployeeDetails();
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

  getEmployeeDetails(): void {
    this.employeeService.getById(this.employeeId).subscribe({
      next: (data) => {
        console.log(data);
        this.employeeForm.patchValue({
          firstName: data.firstName,
          lastName: data.lastName,
          email: data.email,
          phone: data.phone,
          address: data.address,
          dateOfBirth: data.dateOfBirth ? data.dateOfBirth.split('T')[0] : '',
          departmentId: data.departmentId,
          designationId: data.designationId,
        });
      },
    });
  }

  updateEmployee(): void {
    this.employeeService
      .update(this.employeeId, this.employeeForm.value)
      .subscribe({
        next: () => {
          this.router.navigate(['']);
        },
        error: (error) => {
          console.error('Error updating employee:', error);
        },
      });
  }
}
