import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import {
  Designation,
  DesignationService,
} from '../../../service/designation.service';

@Component({
  selector: 'app-designation-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './designation-list.component.html',
  styleUrl: './designation-list.component.css',
})
export class DesignationListComponent implements OnInit {
  designation: Designation[] = [];
  loading: boolean = true;

  constructor(private designationService: DesignationService) {}

  ngOnInit(): void {
    this.fetchDesignation();
  }

  fetchDesignation(): void {
    this.designationService.getAll().subscribe({
      next: (data) => {
        this.designation = data;
        this.loading = false;
        console.log(this.designation);
      },
      error: (err) => {
        console.error('Failded to fetch designation', err);
        this.loading = false;
      },
    });
  }

  deleteDesignation(id: number): void {
    if (confirm('Are you sure you want to delete this designation?')) {
      this.designationService.delete(id).subscribe({
        next: () => {
          this.designation = this.designation.filter(
            (designation) => designation.id != id
          );
        },
        error: (error) => {
          console.error('Error deleting department: ', error);
        },
      });
    }
  }
}
