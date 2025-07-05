import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../services/order.service';
import { OrderModel } from '../../models/order';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-orders-list',
  templateUrl: './orders-list.html',
  styleUrl: './orders-list.css',
  imports: [CommonModule],
})
export class OrdersList implements OnInit {
  orders: OrderModel[] = [];
  loading: boolean = false;

  constructor(private orderService: OrderService) {}

  ngOnInit(): void {
    this.fetchOrders();
  }

  fetchOrders(): void {
    this.loading = true;
    this.orderService.getAllOrders().subscribe({
      next: (data) => {
        this.orders = data;
        console.log('Orders fetched successfully:', this.orders);
        this.loading = false;
      },
      error: (err) => {
        console.error('Error fetching orders:', err);
        this.loading = false;
      },
    });
  }
}
