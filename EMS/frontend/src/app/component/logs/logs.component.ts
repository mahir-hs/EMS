import { Component, OnInit } from '@angular/core';
import { LogsService } from '../../service/logs.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { JsonPrettyPrintModule } from '../../pipes/json-pretty-print/json-pretty-print.module';

@Component({
  selector: 'app-logs',
  standalone: true,
  imports: [CommonModule, FormsModule, JsonPrettyPrintModule],
  templateUrl: './logs.component.html',
  styleUrls: ['./logs.component.css'],
})
export class LogsComponent implements OnInit {
  logs: any[] = []; // All logs fetched from the API
  filteredLogs: any[] = []; // Logs after filtering
  paginatedLogs: any[] = []; // Logs after pagination
  loading: boolean = true;
  search: string = '';
  currentPage: number = 1;
  totalPages: number = 1;
  itemsPerPage: number = 10;

  constructor(private logsService: LogsService) {}

  ngOnInit(): void {
    this.fetchLogs();
  }

  // Fetch all logs from the API
  fetchLogs(): void {
    this.loading = true;
    this.logsService.getAll().subscribe({
      next: (data) => {
        this.logs = data; // Store all logs
        this.filterLogs(); // Apply filter on the full data
        this.loading = false;
      },
      error: (error) => {
        console.error('Error fetching logs:', error);
        this.loading = false;
      },
    });
  }

  // Handle changes in search input
  onSearchChange(): void {
    this.currentPage = 1; // Reset to page 1 on search change
    this.filterLogs(); // Filter logs based on the search term
  }

  // Apply filtering logic based on the search term
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
    this.setPaginatedLogs(); // Apply pagination after filtering
  }

  // Set the logs to be displayed on the current page
  setPaginatedLogs(): void {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedLogs = this.filteredLogs.slice(startIndex, endIndex);
  }

  // Move to the next page
  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.setPaginatedLogs();
    }
  }

  // Move to the previous page
  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.setPaginatedLogs();
    }
  }
}
