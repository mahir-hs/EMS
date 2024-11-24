import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { DepartmentService } from '../../../service/department.service';
import { DesignationService } from '../../../service/designation.service';

@Component({
  selector: 'app-designation-update',
  standalone: true,
  imports: [FormsModule,RouterModule],
  templateUrl: './designation-update.component.html',
  styleUrl: './designation-update.component.css'
})
export class DesignationUpdateComponent implements OnInit{
  designationId: number = 0;
  designation = {
    role:''
  };
  constructor(
    private route:ActivatedRoute,
    private designationService:DesignationService,
    private router:Router
  ){}

  ngOnInit(): void {
    this.designationId = +this.route.snapshot.paramMap.get('id')!;
    this.getDesignationDetails();
  }

  getDesignationDetails():void{
    this.designationService.getById(this.designationId).subscribe({
      next:(data)=>{
        this.designation = data;
      },
      error:(err)=>
      {
        console.error('Error fetching designation details: ',err);
      }
    })
  }

  updataDesignation(): void{
    this.designationService.update(this.designationId,this.designation).subscribe({
      next:()=>{
        this.router.navigate(['designation-list']);
      },
      error:(err)=>{
        console.error('error updaing designation: ',err);
      }
    })
  }
}
