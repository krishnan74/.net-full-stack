import { Inject, inject, Injectable, model } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class PaymentService {
  constructor(private http: HttpClient) {}

  createPayment(
    amount: number,
    currency: string,
    status: string,
    orderId: string,
    razorpayPaymentId: string
  ): Observable<any> {
    return this.http.post<any>('http://localhost:5182/api/Payments', {
      amount,
      currency,
      status,
      orderId,
      razorpayPaymentId,
    });
  }
}
