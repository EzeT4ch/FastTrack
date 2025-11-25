import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, delay } from 'rxjs';
import { environment } from '../../environments/environment';
import { CreateProductRequest, Product } from '../interfaces/product.interface';
import { GridResponse } from '../interfaces/grid-response.interface';
import { GridRequest } from '../interfaces/grid-request.interface';



@Injectable({
    providedIn: 'root'
})
export class ProductService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/products`;

  getProductsFromApi(request: GridRequest, kioskId?: number): Observable<GridResponse<Product>> {
    return this.http.post<GridResponse<Product>>(this.apiUrl + (kioskId ? `?kioskId=${kioskId}` : ''), request);
  }

  saveProduct(product: CreateProductRequest): Observable<string>{
    return this.http.post<string>(`${this.apiUrl}/create`, product);
  }
}