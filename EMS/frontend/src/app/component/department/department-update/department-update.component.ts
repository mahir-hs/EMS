import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { DepartmentService } from '../../../service/department.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-department-update',
  standalone: true,
  imports: [FormsModule, RouterModule, ReactiveFormsModule, CommonModule],
  templateUrl: './department-update.component.html',
  styleUrl: './department-update.component.css',
})
export class DepartmentUpdateComponent implements OnInit {
  departmentId: number = 0;
  // department = {
  //   dept: '',
  // };
  departmentForm!: FormGroup;
  constructor(
    private route: ActivatedRoute,
    private deprtmentService: DepartmentService,
    private router: Router,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.departmentId = +this.route.snapshot.paramMap.get('id')!;
    this.initForm();
    this.getDepartmentDetails();
  }
  initForm(): void {
    this.departmentForm = this.fb.group({
      dept: ['', [Validators.required]],
    });
  }

  getDepartmentDetails(): void {
    this.deprtmentService.getById(this.departmentId).subscribe({
      next: (data) => {
        this.departmentForm.patchValue({
          dpet: data.dept,
        });
      },
      error: (error) => {
        console.error('Error fetching department details: ', error);
      },
    });
  }

  updateDepartment(): void {
    this.deprtmentService
      .update(this.departmentId, this.departmentForm.value)
      .subscribe({
        next: () => {
          this.router.navigate(['department-list']);
        },
        error: (error) => {
          console.error('Error updating department:', error);
        },
      });
  }
}
