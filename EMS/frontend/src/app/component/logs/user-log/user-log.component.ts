import { Component, OnInit } from '@angular/core';
import { LogsService } from '../../../service/logs.service';
import { ActivatedRoute, Route } from '@angular/router';
import { JsonPrettyPrintModule } from '../../../pipes/json-pretty-print/json-pretty-print.module';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-user-log',
  standalone: true,
  imports: [CommonModule, FormsModule, JsonPrettyPrintModule],
  templateUrl: './user-log.component.html',
  styleUrl: './user-log.component.css',
})
export class UserLogComponent implements OnInit {
  userId: number = 0;
  logs: any[] = [];
  paginatedLogs: any[] = [];
  loading: boolean = true;
  currentPage: number = 1;
  totalPages: number = 1;
  itemsPerPage: number = 10;
  constructor(
    private route: ActivatedRoute,
    private logsService: LogsService
  ) {}
  ngOnInit(): void {
    this.userId = +this.route.snapshot.paramMap.get('id')!;
    this.fetchLogs();
  }
  fetchLogs(): void {
    this.loading = true;
    this.logsService.getEmployee(this.userId).subscribe({
      next: (data) => {
        this.logs = data;
        this.setPaginatedLogs();
        this.loading = false;
        console.log(data);
      },
      error: (error) => {
        console.error('Error fetching logs:', error);
        this.loading = false;
      },
    });
  }

  setPaginatedLogs(): void {
    this.totalPages = Math.ceil(this.logs.length / this.itemsPerPage);
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedLogs = this.logs.slice(startIndex, endIndex);
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.setPaginatedLogs();
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.setPaginatedLogs();
    }
  }
}
