import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { DesignationService } from '../../../service/designation.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-designation-update',
  standalone: true,
  imports: [FormsModule, RouterModule, ReactiveFormsModule, CommonModule],
  templateUrl: './designation-update.component.html',
  styleUrl: './designation-update.component.css',
})
export class DesignationUpdateComponent implements OnInit {
  designationId: number = 0;
  // designation = {
  //   role: '',
  // };
  designationForm!: FormGroup;

  constructor(
    private route: ActivatedRoute,
    private designationService: DesignationService,
    private router: Router,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.designationId = +this.route.snapshot.paramMap.get('id')!;
    this.initForm();
    this.getDesignationDetails();
  }

  initForm(): void {
    this.designationForm = this.fb.group({
      role: ['', [Validators.required]],
    });
  }

  getDesignationDetails(): void {
    this.designationService.getById(this.designationId).subscribe({
      next: (data) => {
        this.designationForm.patchValue({
          role: data.role,
        });
      },
      error: (err) => {
        console.error('Error fetching designation details: ', err);
      },
    });
  }

  updataDesignation(): void {
    this.designationService
      .update(this.designationId, this.designationForm.value)
      .subscribe({
        next: () => {
          this.router.navigate(['designation-list']);
        },
        error: (err) => {
          console.error('error updaing designation: ', err);
        },
      });
  }

  cancelDesignation(): void {
    this.designationForm.reset();
    this.router.navigate(['designation-list']);
  }
}
