import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EmployeeService } from '../../../service/employee.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-employee-update',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './employee-update.component.html',
  styleUrls: ['./employee-update.component.css']
})
export class EmployeeUpdateComponent implements OnInit {
  employeeId: number = 0;

  // Explicitly define employee type based on DTO
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

  constructor(
    private route: ActivatedRoute,
    private employeeService: EmployeeService,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Get the ID from the route params
    this.employeeId = +this.route.snapshot.paramMap.get('id')!;
    this.getEmployeeDetails();  // Fetch employee details for the edit form
  }

  getEmployeeDetails(): void {
    this.employeeService.getById(this.employeeId).subscribe({
      next: (data) => {
        this.employee = data;  // Update form data with employee details
      },
      error: (error) => {
        console.error('Error fetching employee details:', error);
      }
    });
  }

  updateEmployee(): void {
    this.employeeService.update(this.employeeId, this.employee).subscribe({
      next: () => {
        this.router.navigate(['']);  // Redirect to employee list
      },
      error: (error) => {
        console.error('Error updating employee:', error);
      }
    });
  }
}
