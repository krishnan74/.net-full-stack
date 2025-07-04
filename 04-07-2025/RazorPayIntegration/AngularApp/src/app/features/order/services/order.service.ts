import { Inject, inject, Injectable, model } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { OrderModel } from '../models/order';
import { API_BASE_URL } from '../../../core/tokens/api-url.token';

@Injectable()
export class OrderService {
  constructor(private http: HttpClient,
    @Inject(API_BASE_URL) private apiBaseUrl: string
  ) {}

  createOrder(
    amount: number,
    customerName: string,
    email: string,
    contactNumber: string
  ): Observable<OrderModel> {
    return this.http.post<any>(`${this.apiBaseUrl}/Orders`, {
      amount,
      customerName,
      email,
      contactNumber,
    });
  }

  getAllOrders(): Observable<OrderModel[]> {
    return this.http.get<OrderModel[]>(`${this.apiBaseUrl}/Orders`);
  }

  getOrderById(id: number): Observable<OrderModel> {
    return this.http.get<OrderModel>(`${this.apiBaseUrl}/Orders/${id}`);
  }
}
