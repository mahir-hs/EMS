import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { DepartmentService } from '../../../service/department.service';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-department-add',
  standalone: true,
  imports: [FormsModule, RouterModule, ReactiveFormsModule, CommonModule],
  templateUrl: './department-add.component.html',
  styleUrl: './department-add.component.css',
})
export class DepartmentAddComponent implements OnInit {
  // department = {
  //   dept: '',
  // };

  departmentForm!: FormGroup;

  constructor(
    private departmentService: DepartmentService,
    private router: Router,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.initForm();
  }

  initForm(): void {
    this.departmentForm = this.fb.group({
      dept: ['', [Validators.required]],
    });
  }

  addDepartment(): void {
    this.departmentService.add(this.departmentForm.value).subscribe({
      next: () => {
        this.router.navigate(['department-list']);
      },
      error: (err) => {
        console.error('Error adding Department', err);
      },
    });
  }

  cancelDepartment(): void {
    this.departmentForm.reset();
    this.router.navigate(['department-list']);
  }
}
