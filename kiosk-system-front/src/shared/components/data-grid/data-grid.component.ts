import {
  Component,
  input,
  output,
  ChangeDetectionStrategy,
  contentChild,
  TemplateRef,
  signal,
  viewChild
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableModule, TableLazyLoadEvent, Table } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { TooltipModule } from 'primeng/tooltip';
import { DatePickerModule } from 'primeng/datepicker';
import { GridRequest } from '../../interfaces/grid-request.interface';

export interface GridColumn {
  field: string;
  header: string;
  sortable?: boolean;
  filterable?: boolean;
  width?: string;
  type?: 'text' | 'date' | 'number' | 'boolean' | 'custom';
}

export interface GridAction {
  icon: string;
  tooltip: string;
  severity?: 'primary' | 'secondary' | 'success' | 'info' | 'warn' | 'danger' | 'help' | 'contrast';
  callback: (row: any) => void;
  visible?: (row: any) => boolean;
}

@Component({
  selector: 'app-data-grid',
  imports: [CommonModule, TableModule, ButtonModule, InputTextModule, TooltipModule, DatePickerModule, FormsModule],
  templateUrl: './data-grid.component.html',
  styleUrl: './data-grid.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DataGridComponent {
  // Inputs
  data = input.required<any[]>();
  columns = input.required<GridColumn[]>();
  loading = input<boolean>(false);
  paginator = input<boolean>(true);
  rows = input<number>(10);
  rowsPerPageOptions = input<number[]>([5, 10, 25, 50, 100]);
  title = input<string>('');
  showAddButton = input<boolean>(true);
  addButtonLabel = input<string>('Nuevo Registro');
  actions = input<GridAction[]>([]);
  emptyMessage = input<string>('No se encontraron registros');
  globalFilterFields = input<string[]>([]);
  striped = input<boolean>(true);
  showCurrentPageReport = input<boolean>(true);
  currentPageReportTemplate = input<string>(
    'Mostrando {first} a {last} de {totalRecords} registros'
  );
  totalRecords = input<number>(0);
  lazy = input<boolean>(false);

  // Outputs
  addClick = output<void>();
  rowSelect = output<any>();
  rowUnselect = output<any>();
  lazyLoad = output<GridRequest>();

  // Template refs
  customColumnTemplate = contentChild<TemplateRef<any>>('customColumn');
  headerTemplate = contentChild<TemplateRef<any>>('headerTemplate');
  table = viewChild<Table>('dt');

  // Internal state
  currentGridRequest = signal<GridRequest>({
    PageNumber: 1,
    PageSize: 10,
    SortColumn: '',
    SortDirection: 'asc',
    Filters: {}
  });
  
  dateRanges = signal<{ [key: string]: Date[] | null }>({});

  onAddClick(): void {
    this.addClick.emit();
  }

  onRowSelect(event: any): void {
    this.rowSelect.emit(event.data);
  }

  onRowUnselect(event: any): void {
    this.rowUnselect.emit(event.data);
  }

  onLazyLoad(event: TableLazyLoadEvent): void {
    if (!this.lazy()) return;

    const gridRequest: GridRequest = {
      PageNumber: Math.floor((event.first || 0) / (event.rows || 10)) + 1,
      PageSize: event.rows || 10,
      SortColumn: (event.sortField as string) || '',
      SortDirection: event.sortOrder === 1 ? 'asc' : 'desc',
      Filters: this.buildFilters(event.filters)
    };

    this.currentGridRequest.set(gridRequest);
    this.lazyLoad.emit(gridRequest);
  }

  private buildFilters(filters: any): { [key: string]: string } {
    if (!filters) return {};

    const result: { [key: string]: string } = {};
    Object.keys(filters).forEach(key => {
      const filter = filters[key];
      if (filter && filter.value !== null && filter.value !== undefined) {
        result[key] = filter.value.toString();
      }
    });
    return result;
  }

  executeAction(action: GridAction, row: any): void {
    action.callback(row);
  }

  isActionVisible(action: GridAction, row: any): boolean {
    return action.visible ? action.visible(row) : true;
  }

  formatValue(value: any, column: GridColumn): string {
    if (!value) return '-';

    switch (column.type) {
      case 'date':
        return new Date(value).toLocaleDateString('es-ES');
      case 'boolean':
        return value ? 'Sí' : 'No';
      case 'number':
        return value.toLocaleString('es-ES');
      default:
        return value;
    }
  }

  clearSort(): void {
    const tableInstance = this.table();
    if (tableInstance) {
      tableInstance.reset();
      
      if (this.lazy()) {
        const gridRequest: GridRequest = {
          ...this.currentGridRequest(),
          SortColumn: '',
          SortDirection: 'asc'
        };
        this.currentGridRequest.set(gridRequest);
        this.lazyLoad.emit(gridRequest);
      }
    }
  }

  hasSorting(): boolean {
    return this.currentGridRequest().SortColumn !== '';
  }

  onFilterChange(field: string, value: string): void {
    const currentRequest = this.currentGridRequest();
    const filters = { ...currentRequest.Filters };
    
    if (value && value.trim()) {
      filters[field] = value.trim();
    } else {
      delete filters[field];
    }
    
    this.currentGridRequest.set({
      ...currentRequest,
      Filters: filters
    });
  }

  onDateFilterChange(field: string, dates: Date[] | null): void {
    const ranges = { ...this.dateRanges() };
    ranges[field] = dates;
    this.dateRanges.set(ranges);

    const currentRequest = this.currentGridRequest();
    const filters = { ...currentRequest.Filters };
    
    // Limpiar filtros de fecha anteriores para este campo
    delete filters[`${field}_from`];
    delete filters[`${field}_to`];
    
    // Agregar nuevos filtros si hay fechas seleccionadas
    if (dates && dates.length === 2 && dates[0] && dates[1]) {
      filters[`${field}_from`] = dates[0].toISOString().split('T')[0];
      filters[`${field}_to`] = dates[1].toISOString().split('T')[0];
    }
    
    this.currentGridRequest.set({
      ...currentRequest,
      Filters: filters
    });
  }

  applyDateFilter(field: string): void {
    if (!this.lazy()) return;
    
    const gridRequest: GridRequest = {
      ...this.currentGridRequest(),
      PageNumber: 1
    };
    
    this.currentGridRequest.set(gridRequest);
    this.lazyLoad.emit(gridRequest);
  }

  applyFilters(): void {
    if (!this.lazy()) return;

    const gridRequest: GridRequest = {
      ...this.currentGridRequest(),
      PageNumber: 1 // Resetear a la primera página al filtrar
    };
    
    this.currentGridRequest.set(gridRequest);
    this.lazyLoad.emit(gridRequest);
  }

  clearFilters(): void {
    const currentRequest = this.currentGridRequest();
    
    this.currentGridRequest.set({
      ...currentRequest,
      PageNumber: 1,
      Filters: {}
    });
    
    this.dateRanges.set({});
    
    if (this.lazy()) {
      this.lazyLoad.emit(this.currentGridRequest());
    }
  }

  hasFilters(): boolean {
    return Object.keys(this.currentGridRequest().Filters).length > 0;
  }
}
