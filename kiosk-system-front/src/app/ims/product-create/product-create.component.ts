import { Component, inject, ChangeDetectionStrategy } from '@angular/core';
import { Router } from '@angular/router';
import { ProductFormComponent } from '../product-form/product-form.component';
import { ProductService } from '../../../shared/services/product.service';
import { CreateProductRequest } from '../../../shared/interfaces/product.interface';

@Component({
  selector: 'app-product-create',
  imports: [ProductFormComponent],
  template: `
    <app-product-form
      (save)="onSave($event)"
      (cancel)="onCancel()"
    />
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProductCreateComponent {
  private router = inject(Router);
  private productService = inject(ProductService);

  onSave(product: CreateProductRequest): void {
     this.productService.saveProduct(product).subscribe({
       next: () => {
         //this.router.navigate(['/products']);
        alert('Producto creado exitosamente (simulado)');

       },
       error: (error) => {
         console.error('Error al guardar producto:', error);
       }
     });
    
    // Por ahora solo navegamos de vuelta
    this.router.navigate(['/products']);
  }

  onCancel(): void {
    this.router.navigate(['/products']);
  }
}
