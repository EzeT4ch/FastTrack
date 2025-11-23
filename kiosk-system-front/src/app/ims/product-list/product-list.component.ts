import { Component, inject, signal, ChangeDetectionStrategy, OnDestroy } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { TagModule } from 'primeng/tag';
import { SelectModule } from 'primeng/select';
import { DataGridComponent, GridAction, GridColumn } from '../../../shared/components/data-grid/data-grid.component';
import { ProductService } from '../../../shared/services/product.service';
import { Product } from '../../../shared/interfaces/product.interface';
import { Subscription } from 'rxjs';
import { GridResponse } from '../../../shared/interfaces/grid-response.interface';
import { GridRequest } from '../../../shared/interfaces/grid-request.interface';

@Component({
  selector: 'app-product-list',
  imports: [DataGridComponent, TagModule, SelectModule, FormsModule],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProductListComponent implements OnDestroy {
  private productService = inject(ProductService);
  private subscription = new Subscription();

  products = signal<Product[]>([]);
  loading = signal<boolean>(false);
  totalRecords = signal<number>(0);
  selectedKioskId = signal<number | null>(null);
  
  kiosks = signal<{ id: number; name: string }[]>([
    { id: 1, name: 'Kiosk Centro - Plaza Mayor' },
    { id: 2, name: 'Kiosk Norte - Terminal' },
    { id: 3, name: 'Kiosk Sur - Aeropuerto' },
    { id: 4, name: 'Kiosk Este - Universidad' },
    { id: 5, name: 'Kiosk Oeste - Parque Central' }
  ]);

  columns: GridColumn[] = [
    { field: 'id', header: 'ID', sortable: true, filterable: true, width: '80px' },
    { field: 'skuCode', header: 'Código', sortable: true, filterable: true },
    { field: 'name', header: 'Nombre', sortable: true, filterable: true },
    { field: 'status', header: 'Estado', sortable: true, type: 'custom' },
    { field: 'dateAdded', header: 'Fecha Registro', sortable: true, type: 'date' },
    { field: 'lastUpdate', header: 'Última Actualización', sortable: true, type: 'date' },
  ];

  actions: GridAction[] = [
    {
      icon: 'pi pi-pencil',
      tooltip: 'Editar',
      severity: 'info',
      callback: (row: Product) => this.editProduct(row),
    },
    {
      icon: 'pi pi-trash',
      tooltip: 'Eliminar',
      severity: 'danger',
      callback: (row: Product) => this.deleteProduct(row),
    },
  ];

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  onLazyLoad(gridRequest: GridRequest): void {
    this.loading.set(true);

    const kioskId = this.selectedKioskId();

    const getProducts: Subscription = this.productService.getProductsFromApi(gridRequest, kioskId ?? undefined).subscribe({
      next: (response: GridResponse<Product>) => {
        this.products.set(response.values);
        this.totalRecords.set(response.totalRecords);
        this.loading.set(false);
      },
      error: (error) => {
        console.error('Error al cargar productos:', error);
        this.loading.set(false);
      }
    });

    this.subscription.add(getProducts);
  }

  onAddProduct(): void {
    console.log('Agregar nuevo producto');
  }

  onKioskChange(kioskId: number | null): void {
    this.selectedKioskId.set(kioskId);
    // Trigger reload with first page
    const gridRequest: GridRequest = {
      PageNumber: 1,
      PageSize: 10,
      SortColumn: '',
      SortDirection: 'asc',
      Filters: {}
    };

    
    this.onLazyLoad(gridRequest);
  }

  editProduct(product: Product): void {
    console.log('Editar producto:', product);
  }

  deleteProduct(product: Product): void {
    if (confirm(`¿Está seguro de eliminar el producto ${product.name}?`)) {
      
    }
  }

  getStatusLabel(status: number): string {
    return status === 1 ? 'Activo' : 'Inactivo';
  }

  getStatusSeverity(status: number): 'success' | 'danger' {
    return status === 1 ? 'success' : 'danger';
  }
}
