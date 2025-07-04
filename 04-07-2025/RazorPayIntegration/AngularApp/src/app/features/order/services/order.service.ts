import { Inject, inject, Injectable, model } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { OrderModel } from '../models/order/order';

@Injectable()
export class OrderService {
  constructor(private http: HttpClient) {}

  createOrder(
    amount: number,
    customerName: string,
    email: string,
    contactNumber: string
  ): Observable<OrderModel> {
    return this.http.post<any>('http://localhost:5182/api/Orders', {
      amount,
      customerName,
      email,
      contactNumber,
    });
  }
}
