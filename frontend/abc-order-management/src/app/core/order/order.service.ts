import { Injectable } from '@angular/core';
import environment from '../../../../environments/environment';
import { lastValueFrom } from 'rxjs';
import { OrderResponse } from '../../shared/types/order/order.interface';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  private readonly API_URL = environment.apiUrlOrders + '/Orders';

  constructor(private http: HttpClient) {}

  async getAllOrders(): Promise<OrderResponse[]> {
    return await lastValueFrom(this.http.get<OrderResponse[]>(this.API_URL));
  }
  
}
