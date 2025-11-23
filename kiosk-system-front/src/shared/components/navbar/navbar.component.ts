import { Component, signal, ChangeDetectionStrategy } from '@angular/core';
import { Router } from '@angular/router';
import { MenubarModule } from 'primeng/menubar';
import { ButtonModule } from 'primeng/button';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-navbar',
  imports: [MenubarModule, ButtonModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class NavbarComponent {
  menuItems = signal<MenuItem[]>([
    {
      label: 'Inventario',
      icon: 'pi pi-box',
      items: [
        {
          label: 'Productos',
          icon: 'pi pi-list',
          command: () => this.navigate('/products')
        },
        {
          label: 'Kioscos',
          icon: 'pi pi-building',
          command: () => this.navigate('/kiosks')
        },
        {
          separator: true
        },
        {
          label: 'Movimientos',
          icon: 'pi pi-arrow-right-arrow-left',
          command: () => this.navigate('/movements')
        }
      ]
    },
    {
      label: 'Órdenes',
      icon: 'pi pi-shopping-cart',
      items: [
        {
          label: 'Nueva Orden',
          icon: 'pi pi-plus',
          command: () => this.navigate('/orders/new')
        },
        {
          label: 'Listado',
          icon: 'pi pi-list',
          command: () => this.navigate('/orders')
        }
      ]
    },
    {
      label: 'Reportes',
      icon: 'pi pi-chart-bar',
      items: [
        {
          label: 'Ventas',
          icon: 'pi pi-dollar',
          command: () => this.navigate('/reports/sales')
        },
        {
          label: 'Inventario',
          icon: 'pi pi-warehouse',
          command: () => this.navigate('/reports/inventory')
        }
      ]
    }
  ]);

  constructor(private router: Router) {}

  navigate(path: string): void {
    this.router.navigate([path]);
  }
}
