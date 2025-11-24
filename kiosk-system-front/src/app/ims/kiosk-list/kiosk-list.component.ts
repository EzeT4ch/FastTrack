import { Component, inject, signal, ChangeDetectionStrategy, OnDestroy } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TagModule } from 'primeng/tag';
import { DataGridComponent, GridAction, GridColumn } from '../../../shared/components/data-grid/data-grid.component';
import { KioskService } from '../../../shared/services/kiosk.service';
import { Kiosk } from '../../../shared/interfaces/kiosk.interface';
import { Subscription } from 'rxjs';
import { GridResponse } from '../../../shared/interfaces/grid-response.interface';
import { GridRequest } from '../../../shared/interfaces/grid-request.interface';

@Component({
  selector: 'app-kiosk-list',
  imports: [DataGridComponent, TagModule, FormsModule],
  templateUrl: './kiosk-list.component.html',
  styleUrl: './kiosk-list.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class KioskListComponent implements OnDestroy {
  private kioskService = inject(KioskService);
  private subscription = new Subscription();

  kiosks = signal<Kiosk[]>([]);
  loading = signal<boolean>(false);
  totalRecords = signal<number>(0);

  columns: GridColumn[] = [
    { field: 'id', header: 'ID', sortable: true, filterable: true, width: '80px' },
    { field: 'code', header: 'Código', sortable: true, filterable: true },
    { field: 'name', header: 'Nombre', sortable: true, filterable: true },
    { field: 'email', header: 'Email', sortable: true, filterable: true },
    { field: 'address', header: 'Dirección', sortable: true, filterable: true },
    { field: 'dateAdded', header: 'Fecha Registro', sortable: true, type: 'date' },
    { field: 'lastUpdate', header: 'Última Actualización', sortable: true, type: 'date' },
  ];

  actions: GridAction[] = [
    {
      icon: 'pi pi-eye',
      tooltip: 'Ver Detalle',
      severity: 'info',
      callback: (row: Kiosk) => this.viewKiosk(row),
    },
    {
      icon: 'pi pi-pencil',
      tooltip: 'Editar',
      severity: 'secondary',
      callback: (row: Kiosk) => this.editKiosk(row),
    },
    {
      icon: 'pi pi-trash',
      tooltip: 'Eliminar',
      severity: 'danger',
      callback: (row: Kiosk) => this.deleteKiosk(row),
    },
  ];

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  onLazyLoad(gridRequest: GridRequest): void {
    this.loading.set(true);

    const getKiosks: Subscription = this.kioskService.getKiosksFromApi(gridRequest).subscribe({
      next: (response: GridResponse<Kiosk>) => {
        this.kiosks.set(response.values);
        this.totalRecords.set(response.totalRecords);
        this.loading.set(false);
      },
      error: (error) => {
        console.error('Error al cargar kiosks:', error);
        this.loading.set(false);
      }
    });

    this.subscription.add(getKiosks);
  }

  onAddKiosk(): void {
  }

  viewKiosk(kiosk: Kiosk): void {
  }

  editKiosk(kiosk: Kiosk): void {
  }

  deleteKiosk(kiosk: Kiosk): void {
    if (confirm(`¿Está seguro de eliminar el kiosk ${kiosk.name}?`)) {
      
      const gridRequest: GridRequest = {
        PageNumber: 1,
        PageSize: 10,
        SortColumn: '',
        SortDirection: 'asc',
        Filters: {}
      };
      this.onLazyLoad(gridRequest);
    }
  }
}
