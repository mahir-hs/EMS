import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DesignationService } from '../../../service/designation.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-designation-add',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './designation-add.component.html',
  styleUrl: './designation-add.component.css'
})
export class DesignationAddComponent {
 designation = {
  role:''
 };

 constructor(private designationService:DesignationService,private router:Router){}

 addDesignation():void{
  this.designationService.add(this.designation).subscribe({
    next:()=>{
      this.router.navigate(['designation-list']);
    },
    error:(err)=>{
      console.error('Error adding Department',err);
    }
  })
}
}
