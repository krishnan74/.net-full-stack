import { Inject, inject, Injectable, model } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_BASE_URL } from '../../../core/tokens/api-url.token';

@Injectable()
export class PaymentService {
  constructor(private http: HttpClient, 
    @Inject(API_BASE_URL) private apiBaseUrl: string
  ) {}


  createPayment(
    amount: number,
    currency: string,
    status: string,
    orderId: string,
    razorpayPaymentId: string
  ): Observable<any> {
    console.log('Creating payment with details:', {
      amount,
      currency,
      status,
      orderId,
      razorpayPaymentId,
    });
    return this.http.post<any>(`${this.apiBaseUrl}/Payments`, {
      amount,
      currency,
      status,
      orderId,
      razorpayPaymentId,
    });
  }

  getAllPayments(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiBaseUrl}/Payments`);
  }

  getPaymentById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiBaseUrl}/Payments/${id}`);
  }
}
