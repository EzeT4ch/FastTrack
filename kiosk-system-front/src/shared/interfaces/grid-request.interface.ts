export interface GridRequest {
  PageNumber: number;
  PageSize: number;
  SortColumn: string;
  SortDirection: string;
  Filters: { [key: string]: string };
}

