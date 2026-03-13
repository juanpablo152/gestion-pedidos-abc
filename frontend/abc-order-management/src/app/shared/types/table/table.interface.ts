export type ColumnFormatType = 'text' | 'currency' | 'number' | 'date' | 'percent' | 'badge';

export interface ColumnFormat {
  type: ColumnFormatType;
  currency?: string;
  locale?: string;
  digitsInfo?: string;
  dateFormat?: string;
}

export interface TableColumn<T = any> {
  key: string;
  label: string;
  width?: string;
  format?: ColumnFormat;
  formatter?: (value: any, row: T) => string;
}
