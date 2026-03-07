export interface TableColumn<T = any> {
  key: string;
  label: string;
  width?: string;
  formatter?: (value: any, row: T) => string;
}
