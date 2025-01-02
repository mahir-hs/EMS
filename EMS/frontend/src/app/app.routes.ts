import { Routes } from '@angular/router';
import { EmployeeListComponent } from './component/employee/employee-list/employee-list.component';
import { EmployeeAddComponent } from './component/employee/employee-add/employee-add.component';
import { EmployeeUpdateComponent } from './component/employee/employee-update/employee-update.component';
import { DepartmentAddComponent } from './component/department/department-add/department-add.component';
import { DepartmentListComponent } from './component/department/department-list/department-list.component';
import { DepartmentUpdateComponent } from './component/department/department-update/department-update.component';
import { DesignationAddComponent } from './component/designation/designation-add/designation-add.component';
import { DesignationListComponent } from './component/designation/designation-list/designation-list.component';
import { DesignationUpdateComponent } from './component/designation/designation-update/designation-update.component';
import { AttendanceAddComponent } from './component/attendance/attendance-add/attendance-add.component';
import { AttendanceListComponent } from './component/attendance/attendance-list/attendance-list.component';
import { AttendanceUpdateComponent } from './component/attendance/attendance-update/attendance-update.component';

import { AttendanceUserComponent } from './component/attendance/attendance-user/attendance-user.component';
import { AllLogComponent } from './component/logs/all-log/all-log.component';
import { UserLogComponent } from './component/logs/user-log/user-log.component';
import { AttendanceLogComponent } from './component/logs/attendance-log/attendance-log.component';
import { LoginComponent } from './component/auth/login/login.component';
import { RegisterComponent } from './component/auth/register/register.component';
import { authGuard } from './AuthGuard/auth.guard';
import { redirectLoggedinGuard } from './AuthGuard/redirect-loggedin.guard';
import { ResetPasswordComponent } from './component/auth/reset-password/reset-password.component';
import { RequestPasswordResetComponent } from './component/auth/request-password-reset/request-password-reset.component';

export const routes: Routes = [
  { path: '', component: EmployeeListComponent, canActivate: [authGuard] },
  {
    path: 'employee-add',
    component: EmployeeAddComponent,
    canActivate: [authGuard],
    data: { role: 'Admin' },
  },
  {
    path: 'employee-update/:id',
    component: EmployeeUpdateComponent,
    canActivate: [authGuard],
    data: { role: 'Employee' },
  },
  {
    path: 'department-add',
    component: DepartmentAddComponent,
    canActivate: [authGuard],
    data: { role: 'Admin' },
  },
  {
    path: 'department-list',
    component: DepartmentListComponent,
    canActivate: [authGuard],
    data: { role: 'Employee' },
  },
  {
    path: 'department-update/:id',
    component: DepartmentUpdateComponent,
    canActivate: [authGuard],
    data: { role: 'Admin' },
  },
  {
    path: 'designation-add',
    component: DesignationAddComponent,
    canActivate: [authGuard],
    data: { role: 'Admin' },
  },
  {
    path: 'designation-list',
    component: DesignationListComponent,
    canActivate: [authGuard],
    data: { role: 'Employee' },
  },
  {
    path: 'designation-update/:id',
    component: DesignationUpdateComponent,
    canActivate: [authGuard],
    data: { role: 'Admin' },
  },
  {
    path: 'attendance-add/:id',
    component: AttendanceAddComponent,
    canActivate: [authGuard],
    data: { role: 'Employee' },
  },
  {
    path: 'attendance-list',
    component: AttendanceListComponent,
    canActivate: [authGuard],
    data: { role: 'Admin' },
  },
  {
    path: 'attendance-update/:id',
    component: AttendanceUpdateComponent,
    canActivate: [authGuard],
    data: { role: 'Employee' },
  },
  {
    path: 'attendance-user/:id',
    component: AttendanceUserComponent,
    canActivate: [authGuard],
    data: { role: 'Employee' },
  },
  {
    path: 'log-list',
    component: AllLogComponent,
    canActivate: [authGuard],
    data: { role: 'Admin' },
  },
  {
    path: 'log-user/:id',
    component: UserLogComponent,
    canActivate: [authGuard],
    data: { role: 'Employee' },
  },
  {
    path: 'log-attendance/:id',
    component: AttendanceLogComponent,
    canActivate: [authGuard],
    data: { role: 'Employee' },
  },
  {
    path: 'login',
    component: LoginComponent,
    canActivate: [redirectLoggedinGuard],
  },
  {
    path: 'register',
    component: RegisterComponent,
    canActivate: [redirectLoggedinGuard],
  },
  { path: 'reset-password', component: ResetPasswordComponent },
  { path: 'request-password-reset', component: RequestPasswordResetComponent },
];
