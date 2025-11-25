import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./home/home.component').then(m => m.HomeComponent)
  },
  {
    path: 'products',
    loadComponent: () => import('./ims/product-list/product-list.component').then(m => m.ProductListComponent)
  },
  {
    path: 'products/new',
    loadComponent: () => import('./ims/product-create/product-create.component').then(m => m.ProductCreateComponent)
  },
  {
    path: 'kiosks',
    loadComponent: () => import('./ims/kiosk-list/kiosk-list.component').then(m => m.KioskListComponent)
  }
];
