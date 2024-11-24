import { EmployeeService } from './../../../service/employee.service';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
@Component({
  selector: 'app-employee-add',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './employee-add.component.html',
  styleUrls: ['./employee-add.component.css']
})
export class EmployeeAddComponent {
  employee = {
    firstName: '',
    lastName: '',
    email: '',
    phone: '',
    address: '',
    dateOfBirth: '',
    departmentId: null,
    designationId: null 
  };

  constructor(private employeeService: EmployeeService, private router: Router) {}

  // Add employee method
  addEmployee(): void {
    // Call the service to add employee and navigate on success
    this.employeeService.add(this.employee).subscribe({
      next: () => {
        this.router.navigate(['']);  // Redirect to employee list
      },
      error: (err) => {
        console.error('Error adding employee', err);  // Handle error if needed
      }
    });
  }
}
