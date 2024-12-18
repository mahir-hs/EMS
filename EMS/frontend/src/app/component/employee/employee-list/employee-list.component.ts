import { DownloadService } from './../../../service/download.service';
import { Component, OnInit } from '@angular/core';
import { EmployeeService, Employee } from '../../../service/employee.service';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { DepartmentService } from '../../../service/department.service';
import { DesignationService } from '../../../service/designation.service';
import { AttendanceService } from '../../../service/attendance.service';
import * as XLSX from 'xlsx';
import { ngxCsv } from 'ngx-csv';
@Component({
  selector: 'app-employee-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.css'],
})
export class EmployeeListComponent implements OnInit {
  employees: Employee[] = [];
  departments: any[] = [];
  designations: any[] = [];
  paginatedEmployees: Employee[] = [];

  loading: boolean = true;
  itemsPerPage: number = 10;
  currentPage: number = 1;
  totalPages: number = 1;
  constructor(
    private employeeService: EmployeeService,
    private departmentService: DepartmentService,
    private designationService: DesignationService,
    private downloadService: DownloadService
  ) {}

  ngOnInit(): void {
    this.fetchEmployees();
    this.fetchDepartments();
    this.fetchDesignations();
  }

  fetchEmployees(): void {
    this.employeeService.getAll().subscribe({
      next: (data) => {
        this.employees = data;
        this.loading = false;
        this.totalPages = Math.ceil(this.employees.length / this.itemsPerPage);
        this.setPaginatedEmployees();
        console.log(this.employees);
      },
      error: (err) => {
        console.error('Failed to fetch employees:', err);
        this.loading = false;
      },
    });
  }

  sortKey: string = '';
  sortDirection: boolean = true;

  sortEmployees(key: string): void {
    if (this.sortKey === key) {
      this.sortDirection = !this.sortDirection;
    } else {
      this.sortKey = key;
      this.sortDirection = true;
    }
    this.employees.sort((a: any, b: any) => {
      if (a[key] < b[key]) return this.sortDirection ? -1 : 1;
      if (a[key] > b[key]) return this.sortDirection ? 1 : -1;
      return 0;
    });
    this.setPaginatedEmployees();
  }
  setPaginatedEmployees(): void {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedEmployees = this.employees.slice(startIndex, endIndex);
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.setPaginatedEmployees();
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.setPaginatedEmployees();
    }
  }
  fetchDepartments(): void {
    this.departmentService.getAll().subscribe({
      next: (data) => {
        this.departments = data;
      },
      error: (err) => {
        console.error('Failed to fetch departments:', err);
      },
    });
  }

  fetchDesignations(): void {
    this.designationService.getAll().subscribe({
      next: (data) => {
        this.designations = data;
      },
      error: (err) => {
        console.error('Failed to fetch designations:', err);
      },
    });
  }

  getDepartmentName(departmentId: number | undefined): string {
    if (!departmentId) {
      return 'N/A';
    }
    const department = this.departments.find(
      (dept) => dept.id === departmentId
    );
    return department ? department.dept : 'N/A';
  }

  getDesignationName(designationId: number | undefined): string {
    if (!designationId) {
      return 'N/A';
    }
    const designation = this.designations.find(
      (des) => des.id === designationId
    );
    return designation ? designation.role : 'N/A';
  }

  deleteEmployee(id: number): void {
    if (confirm('Are you sure you want to delete this employee?')) {
      this.employeeService.delete(id).subscribe({
        next: () => {
          this.employees = this.employees.filter(
            (employee) => employee.id !== id
          );

          this.totalPages = Math.ceil(
            this.employees.length / this.itemsPerPage
          );

          if (this.currentPage > this.totalPages) {
            this.currentPage = this.totalPages || 1;
          }

          this.setPaginatedEmployees();
        },
        error: (error) => {
          console.error('Error deleting employee:', error);
        },
      });
    }
  }

  exportToCSV(): void {
    this.downloadService.employeeExportCsv().subscribe({
      next: (response) => {
        const blob = new Blob([response], { type: 'text/csv' });
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = 'EmployeeList.csv';
        a.click();
        window.URL.revokeObjectURL(url);
      },
      error: (err) => {
        console.error('Failed to download CSV:', err);
      },
    });
  }

  exportToExcel(): void {
    this.downloadService.employeeExportExcel().subscribe({
      next: (response) => {
        const blob = new Blob([response], {
          type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
        });
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = 'EmployeeList.xlsx';
        a.click();
        window.URL.revokeObjectURL(url);
      },
      error: (err) => {
        console.error('Failed to download Excel:', err);
      },
    });
  }
}
