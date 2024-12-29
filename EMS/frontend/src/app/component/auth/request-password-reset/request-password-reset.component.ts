import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { AuthService } from '../../../service/auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-request-password-reset',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './request-password-reset.component.html',
  styleUrl: './request-password-reset.component.css',
})
export class RequestPasswordResetComponent {
  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {
    this.emailForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
    });
  }
  emailForm: FormGroup;
  errorMessage: string = '';
  successMessage: string = '';
  onSubmit(): void {
    if (this.emailForm.invalid) {
      return;
    }

    const email = this.emailForm.value.email;

    this.authService.requestPasswordReset(email).subscribe({
      next: (response) => {
        this.successMessage =
          'If this email exists, you will receive a reset link shortly.';
        this.emailForm.reset();
        this.router.navigate(['/login']);
      },
      error: (error) => {
        this.errorMessage =
          'Failed to send reset password email. Please try again.';
      },
    });
  }
}
