import { Component, inject, signal, ChangeDetectionStrategy, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { InputNumberModule } from 'primeng/inputnumber';
import { TextareaModule } from 'primeng/textarea';
import { SelectModule } from 'primeng/select';
import { ButtonModule } from 'primeng/button';
import { CardModule } from 'primeng/card';
import { KioskService } from '../../../shared/services/kiosk.service';
import { Subscription } from 'rxjs';
import { KioskLabel } from '../../../shared/interfaces/kiosk.interface';
import { CreateProductRequest } from '../../../shared/interfaces/product.interface';

@Component({
  selector: 'app-product-form',
  imports: [
    FormsModule,
    InputTextModule,
    InputNumberModule,
    TextareaModule,
    SelectModule,
    ButtonModule,
    CardModule
  ],
  templateUrl: './product-form.component.html',
  styleUrl: './product-form.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProductFormComponent {
  private kioskService = inject(KioskService);
  private subscription = new Subscription();

  // Outputs
  save = output<CreateProductRequest>();
  cancel = output<void>();

  // Form data
  product = signal<CreateProductRequest>({
    skuCode: '',
    name: '',
    description: '',
    price: 0,
    kioskId: 0,
    status: 1,
    initialStock: 0
  });

  kiosks = signal<KioskLabel[]>([]);

  statusOptions = signal([
    { id: 1, name: 'Activo' },
    { id: 0, name: 'Inactivo' }
  ]);

  // Validation
  errors = signal<{ [key: string]: string }>({});

  onSubmit(): void {
    if (this.validateForm()) {
      this.save.emit(this.product());
    }
  }

  onCancel(): void {
    this.cancel.emit();
  }

  updateField<K extends keyof CreateProductRequest>(field: K, value: CreateProductRequest[K]): void {
    this.product.set({
      ...this.product(),
      [field]: value
    });
    
    const currentErrors = { ...this.errors() };
    delete currentErrors[field as string];
    this.errors.set(currentErrors);
  }

  private validateForm(): boolean {
    const newErrors: { [key: string]: string } = {};
    const prod = this.product();

    if (!prod.skuCode || prod.skuCode.trim() === '') {
      newErrors['skuCode'] = 'El código SKU es requerido';
    }

    if (!prod.name || prod.name.trim() === '') {
      newErrors['name'] = 'El nombre es requerido';
    }

    if (!prod.description || prod.description.trim() === '') {
      newErrors['description'] = 'La descripción es requerida';
    }

    if (prod.price <= 0) {
      newErrors['price'] = 'El precio debe ser mayor a 0';
    }

    if (!prod.kioskId || prod.kioskId === 0) {
      newErrors['kioskId'] = 'Debe seleccionar un kiosk';
    }

    if (prod.initialStock !== undefined && prod.initialStock < 0) {
      newErrors['initialStock'] = 'El stock inicial no puede ser negativo';
    }

    this.errors.set(newErrors);
    return Object.keys(newErrors).length === 0;
  }

   private fetchKiosks(): void {
      const getKiosks: Subscription = this.kioskService.getKioksForLabels().subscribe({
        next: (kioskLabels: KioskLabel[]) => {
          this.kiosks.set(kioskLabels);
        },
        error: (error) => {
          console.error('Error al cargar kioscos:', error);
        }
      });
  
      this.subscription.add(getKiosks);
    }
}
