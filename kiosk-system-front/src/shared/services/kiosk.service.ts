import { inject, Injectable, signal } from '@angular/core';
import { Observable, of, delay } from 'rxjs';
import { Kiosk } from '../interfaces/kiosk.interface';
import { GridRequest } from '../interfaces/grid-request.interface';
import { GridResponse } from '../interfaces/grid-response.interface';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class KioskService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/kiosks`;
  
  getKiosksFromApi(gridRequest: GridRequest): Observable<GridResponse<Kiosk>> {
    return this.http.post<GridResponse<Kiosk>>(this.apiUrl, gridRequest);
  }
}
