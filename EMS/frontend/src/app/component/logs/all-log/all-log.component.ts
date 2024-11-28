import { Component, OnInit } from '@angular/core';
import { LogsService } from '../../../service/logs.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { JsonPrettyPrintModule } from '../../../pipes/json-pretty-print/json-pretty-print.module';

@Component({
  selector: 'app-all-log',
  standalone: true,
  imports: [CommonModule, FormsModule, JsonPrettyPrintModule],
  templateUrl: './all-log.component.html',
  styleUrl: './all-log.component.css',
})
export class AllLogComponent implements OnInit {
  logs: any[] = [];
  filteredLogs: any[] = [];
  paginatedLogs: any[] = [];
  loading: boolean = true;
  search: string = '';
  currentPage: number = 1;
  totalPages: number = 1;
  itemsPerPage: number = 10;

  constructor(private logsService: LogsService) {}

  ngOnInit(): void {
    this.fetchLogs();
  }

  fetchLogs(): void {
    this.loading = true;
    this.logsService.getAll().subscribe({
      next: (data) => {
        this.logs = data;
        this.filterLogs();
        this.loading = false;
      },
      error: (error) => {
        console.error('Error fetching logs:', error);
        this.loading = false;
      },
    });
  }

  onSearchChange(): void {
    this.currentPage = 1;
    this.filterLogs();
  }

  filterLogs(): void {
    this.filteredLogs = this.logs.filter(
      (log) =>
        log.operationType.toLowerCase().includes(this.search.toLowerCase()) ||
        log.operationDetails
          .toLowerCase()
          .includes(this.search.toLowerCase()) ||
        log.entityName.toLowerCase().includes(this.search.toLowerCase())
    );
    this.totalPages = Math.ceil(this.filteredLogs.length / this.itemsPerPage); // Recalculate total pages
    this.setPaginatedLogs();
  }

  setPaginatedLogs(): void {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedLogs = this.filteredLogs.slice(startIndex, endIndex);
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
