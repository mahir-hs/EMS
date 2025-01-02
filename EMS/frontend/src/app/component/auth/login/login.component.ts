import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { AuthService } from '../../../service/auth.service';
import { Router, RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    RouterModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    CommonModule,
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent implements OnInit {
  loginForm!: FormGroup;
  errorMessage: string = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router
  ) {}
  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      rememberMe: [false],
    });
    this.populateRememberedData();
  }
  populateRememberedData() {
    const rememberedEmail = localStorage.getItem('email');
    const rememberedPassword = localStorage.getItem('password');

    if (rememberedEmail && rememberedPassword) {
      this.loginForm.patchValue({
        email: rememberedEmail,
        password: rememberedPassword,
        rememberMe: true,
      });
    }
  }

  onSubmit() {
    if (this.loginForm.valid) {
      this.authService.login(this.loginForm.value).subscribe({
        next: (res: any) => {
          console.log(res);
          this.authService.clearSession();
          this.authService.setToken(res.accessToken);
          this.authService.setRefreshToken(res.refreshToken);
          this.authService.setRole(res.accessToken);
          const rememberMe = this.loginForm.get('rememberMe')?.value;
          const email = this.loginForm.get('email')?.value;
          const password = this.loginForm.get('password')?.value;
          console.log(rememberMe);
          if (rememberMe) {
            localStorage.setItem('rememberMe', 'true');
            localStorage.setItem('password', password);
            localStorage.setItem('email', email);
          } else {
            localStorage.removeItem('rememberMe');
            localStorage.removeItem('email');
            localStorage.removeItem('password');
          }
          this.router.navigate(['/']);
        },
        error: (err) =>
          (this.errorMessage = err.error.message || 'Login failed'),
      });
    }
  }
}
