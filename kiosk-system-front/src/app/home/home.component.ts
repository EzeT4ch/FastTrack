import { Component, signal, ChangeDetectionStrategy } from '@angular/core';
import { Router } from '@angular/router';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';

interface QuickAccessCard {
  title: string;
  description: string;
  icon: string;
  route: string;
  color: string;
}

@Component({
  selector: 'app-home',
  imports: [CardModule, ButtonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HomeComponent {
  quickAccessCards = signal<QuickAccessCard[]>([
    {
      title: 'Productos',
      description: 'Gestiona el catálogo de productos del sistema',
      icon: 'pi-box',
      route: '/products',
      color: '#3b82f6'
    },
    {
      title: 'Kioscos',
      description: 'Administra los kioscos y sus ubicaciones',
      icon: 'pi-building',
      route: '/kiosks',
      color: '#8b5cf6'
    },
    {
      title: 'Nueva Orden',
      description: 'Crea una nueva orden de compra',
      icon: 'pi-shopping-cart',
      route: '/orders/new',
      color: '#10b981'
    },
    {
      title: 'Movimientos',
      description: 'Consulta movimientos de inventario',
      icon: 'pi-arrow-right-arrow-left',
      route: '/movements',
      color: '#f59e0b'
    },
    {
      title: 'Órdenes',
      description: 'Visualiza todas las órdenes registradas',
      icon: 'pi-list',
      route: '/orders',
      color: '#ec4899'
    },
    {
      title: 'Reportes',
      description: 'Genera reportes de ventas e inventario',
      icon: 'pi-chart-bar',
      route: '/reports',
      color: '#06b6d4'
    }
  ]);

  constructor(private router: Router) {}

  navigateTo(route: string): void {
    this.router.navigate([route]);
  }
}
