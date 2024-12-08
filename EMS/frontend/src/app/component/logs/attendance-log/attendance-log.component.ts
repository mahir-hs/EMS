import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { JsonPrettyPrintModule } from '../../../pipes/json-pretty-print/json-pretty-print.module';
import { ActivatedRoute } from '@angular/router';
import { LogsService } from '../../../service/logs.service';

@Component({
  selector: 'app-attendance-log',
  standalone: true,
  imports: [CommonModule, FormsModule, JsonPrettyPrintModule],
  templateUrl: './attendance-log.component.html',
  styleUrl: './attendance-log.component.css',
})
export class AttendanceLogComponent implements OnInit {
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
    this.logsService.getAttendance(this.userId).subscribe({
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
