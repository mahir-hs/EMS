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

export const routes: Routes = [
  {path: '', component:EmployeeListComponent},
  {path:'employee-add',component:EmployeeAddComponent},
  {path: 'employee-update/:id', component: EmployeeUpdateComponent },
  {path:'department-add',component:DepartmentAddComponent},
  {path:'department-list',component:DepartmentListComponent},
  {path:'department-update/:id',component:DepartmentUpdateComponent},
  {path:'designation-add',component:DesignationAddComponent},
  {path:'designation-list',component:DesignationListComponent},
  {path:'designation-update/:id',component:DesignationUpdateComponent},
  {path:'attendance-add',component:AttendanceAddComponent},
  {path:'attendance-list',component:AttendanceListComponent},
  {path:'attendance-update/:id',component:AttendanceUpdateComponent},

];


