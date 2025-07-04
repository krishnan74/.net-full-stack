import { Component, OnInit } from '@angular/core';
import { PaymentService } from '../../services/payment.service';
import { PaymentModel } from '../../models/payment';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-payments-list',
  templateUrl: './payments-list.html',
  styleUrl: './payments-list.css',
  imports: [CommonModule],
})
export class PaymentsList implements OnInit {
  payments: PaymentModel[] = [];
  loading: boolean = false;

  constructor(private paymentService: PaymentService) {}

  ngOnInit(): void {
    this.fetchPayments();
  }

  fetchPayments(): void {
    this.loading = true;
    this.paymentService.getAllPayments().subscribe({
      next: (data) => {
        this.payments = data;
        console.log('Payments fetched successfully:', this.payments);
        this.loading = false;
      },
      error: (err) => {
        console.error('Error fetching payments:', err);
        this.loading = false;
      },
    });
  }
}
