import { Component, OnInit } from '@angular/core';
import {
  Form,
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { DesignationService } from '../../../service/designation.service';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-designation-add',
  standalone: true,
  imports: [FormsModule, RouterModule, ReactiveFormsModule, CommonModule],
  templateUrl: './designation-add.component.html',
  styleUrl: './designation-add.component.css',
})
export class DesignationAddComponent implements OnInit {
  // designation = {
  //   role: '',
  // };

  designationForm!: FormGroup;

  constructor(
    private designationService: DesignationService,
    private router: Router,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.initForm();
  }

  initForm(): void {
    this.designationForm = this.fb.group({
      role: ['', [Validators.required]],
    });
  }

  addDesignation(): void {
    this.designationService.add(this.designationForm.value).subscribe({
      next: () => {
        this.router.navigate(['designation-list']);
      },
      error: (err) => {
        console.error('Error adding Designation', err);
      },
    });
  }

  cancelDesignation(): void {
    this.designationForm.reset();
    this.router.navigate(['designation-list']);
  }
}
