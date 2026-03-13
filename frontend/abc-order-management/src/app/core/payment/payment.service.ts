import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import environment from '../../../../environments/environment';
import { OrderResponse } from '../../shared/types/order/order.interface';
import { PaymentResponse } from '../../shared/types/payment/payment.interface';

@Injectable({
  providedIn: 'root',
})
export class PaymentService {
  private readonly API_URL = environment.apiUrlPayments + '/Payments';

  constructor(private http: HttpClient) {}

  async getAllPaymentsWithOrderInfo(): Promise<PaymentResponse[]> {
    return await lastValueFrom(this.http.get<PaymentResponse[]>(`${this.API_URL}/info`));
  }

  async getAllPayments(): Promise<PaymentResponse[]> {
    return await lastValueFrom(this.http.get<PaymentResponse[]>(`${this.API_URL}`));
  }
}
