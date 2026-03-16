import { Injectable } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import environment from '../../../../../environments/environment';
import { OrderResponse } from '../../../shared/types/order/order.interface';

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
