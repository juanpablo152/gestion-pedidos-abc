import { Component, Input, inject } from '@angular/core';
import {
  CommonModule,
  CurrencyPipe,
  DatePipe,
  DecimalPipe,
  PercentPipe,
} from '@angular/common';
import { ColumnFormat, TableColumn } from '../../../types/table/table.interface';

@Component({
  selector: 'ui-table',
  standalone: true,
  imports: [CommonModule],
  providers: [CurrencyPipe, DatePipe, DecimalPipe, PercentPipe],
  templateUrl: './table.html',
  styleUrl: './table.scss',
})
export class TableComponent {
  @Input() columns: TableColumn[] = [];
  @Input() data: any[] = [];
  @Input() loading = false;
  @Input() emptyMessage = 'No hay datos disponibles';
  @Input() title = '';

  private currencyPipe = inject(CurrencyPipe);
  private datePipe = inject(DatePipe);
  private decimalPipe = inject(DecimalPipe);
  private percentPipe = inject(PercentPipe);

  getCellValue(row: any, col: TableColumn): string {
    const value = this.resolveValue(row, col.key);
    if (col.formatter) return col.formatter(value, row);
    if (col.format) return this.applyFormat(value, col.format);
    return value !== null && value !== undefined ? String(value) : '—';
  }

  isBadge(col: TableColumn): boolean {
    return col.format?.type === 'badge';
  }

  getBadgeClass(row: any, col: TableColumn): string {
    const value = this.resolveValue(row, col.key);
    return `badge badge--${String(value).toLowerCase().replace(/\s+/g, '-')}`;
  }

  private applyFormat(value: any, format: ColumnFormat): string {
    if (value === null || value === undefined) return '—';

    switch (format.type) {
      case 'currency':
        return (
          this.currencyPipe.transform(
            value,
            format.currency ?? 'USD',
            'symbol',
            format.digitsInfo ?? '1.2-2',
            format.locale,
          ) ?? '—'
        );

      case 'number':
        return (
          this.decimalPipe.transform(
            value,
            format.digitsInfo ?? '1.0-2',
            format.locale,
          ) ?? '—'
        );

      case 'percent':
        return (
          this.percentPipe.transform(
            value,
            format.digitsInfo ?? '1.0-2',
            format.locale,
          ) ?? '—'
        );

      case 'date':
        return (
          this.datePipe.transform(
            value,
            format.dateFormat ?? 'dd/MM/yyyy',
            undefined,
            format.locale,
          ) ?? '—'
        );

      case 'badge':
        return String(value);

      default:
        return String(value);
    }
  }

  private resolveValue(obj: any, path: string): any {
    return path.split('.').reduce((acc, key) => acc?.[key], obj);
  }

  trackByIndex(index: number): number {
    return index;
  }
}
