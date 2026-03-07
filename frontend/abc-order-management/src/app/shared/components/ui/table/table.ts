import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TableColumn } from '../../../types/table/table.interface';

@Component({
  selector: 'ui-table',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './table.html',
  styleUrl: './table.scss',
})
export class TableComponent {
  @Input() columns: TableColumn[] = [];
  @Input() data: any[] = [];
  @Input() loading = false;
  @Input() emptyMessage = 'No hay datos disponibles';
  @Input() title = '';

  skeletonRows = [1, 2, 3, 4, 5];

  getCellValue(row: any, col: TableColumn): string {
    const value = row[col.key];
    if (col.formatter) return col.formatter(value, row);
    return value ?? '—';
  }
}
