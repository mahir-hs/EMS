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

export const routes: Routes = [
  { path: '', component: EmployeeListComponent, canActivate: [authGuard] },
  {
    path: 'employee-add',
    component: EmployeeAddComponent,
    canActivate: [authGuard],
  },
  {
    path: 'employee-update/:id',
    component: EmployeeUpdateComponent,
    canActivate: [authGuard],
  },
  {
    path: 'department-add',
    component: DepartmentAddComponent,
    canActivate: [authGuard],
  },
  {
    path: 'department-list',
    component: DepartmentListComponent,
    canActivate: [authGuard],
  },
  {
    path: 'department-update/:id',
    component: DepartmentUpdateComponent,
    canActivate: [authGuard],
  },
  {
    path: 'designation-add',
    component: DesignationAddComponent,
    canActivate: [authGuard],
  },
  {
    path: 'designation-list',
    component: DesignationListComponent,
    canActivate: [authGuard],
  },
  {
    path: 'designation-update/:id',
    component: DesignationUpdateComponent,
    canActivate: [authGuard],
  },
  {
    path: 'attendance-add/:id',
    component: AttendanceAddComponent,
    canActivate: [authGuard],
  },
  {
    path: 'attendance-list',
    component: AttendanceListComponent,
    canActivate: [authGuard],
  },
  {
    path: 'attendance-update/:id',
    component: AttendanceUpdateComponent,
    canActivate: [authGuard],
  },
  {
    path: 'attendance-user/:id',
    component: AttendanceUserComponent,
    canActivate: [authGuard],
  },
  { path: 'log-list', component: AllLogComponent, canActivate: [authGuard] },
  {
    path: 'log-user/:id',
    component: UserLogComponent,
    canActivate: [authGuard],
  },
  {
    path: 'log-attendance/:id',
    component: AttendanceLogComponent,
    canActivate: [authGuard],
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
];
