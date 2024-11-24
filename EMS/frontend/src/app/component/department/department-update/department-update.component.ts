import { EmployeeService } from './../../../service/employee.service';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { DepartmentService } from '../../../service/department.service';

@Component({
  selector: 'app-department-update',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './department-update.component.html',
  styleUrl: './department-update.component.css'
})
export class DepartmentUpdateComponent implements OnInit{
  departmentId: number = 0;
  department = {
    dept: ''
  };
  constructor(
    private route:ActivatedRoute,
    private deprtmentService:DepartmentService,
    private router:Router
  ){}

  ngOnInit(): void {
    this.departmentId = +this.route.snapshot.paramMap.get('id')!;
    this.getDepartmentDetails();
  }

  getDepartmentDetails():void{
    this.deprtmentService.getById(this.departmentId).subscribe({
      next:(data) => {
        this.department = data;
      },
      error:(error)=>{
        console.error('Error fetching department details: ',error);
      }
    });
  }

  updateDepartment(): void {
    this.deprtmentService.update(this.departmentId, this.department).subscribe({
      next: () => {
        this.router.navigate(['department-list']);
      },
      error: (error) => {
        console.error('Error updating department:', error);
      }
    });
  }

}
