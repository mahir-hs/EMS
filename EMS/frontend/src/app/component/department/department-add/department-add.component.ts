import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DepartmentService } from '../../../service/department.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-department-add',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './department-add.component.html',
  styleUrl: './department-add.component.css'
})
export class DepartmentAddComponent {
  department = {
    dept:''
  };

  constructor(private departmentService:DepartmentService,private router:Router){}

  addDepartment():void{
    this.departmentService.add(this.department).subscribe({
      next:()=>{
        this.router.navigate(['department-list']);
      },
      error:(err)=>{
        console.error('Error adding Department',err);
      }
    })
  }

}
