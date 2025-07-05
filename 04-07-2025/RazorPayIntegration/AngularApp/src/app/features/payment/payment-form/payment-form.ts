import { Component } from '@angular/core';
import {
  FormGroup,
  FormBuilder,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { RazorpayService } from '../../razorpay/razorpay.service';
import { OrderService } from '../../order/services/order.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-payment-form',
  templateUrl: './payment-form.html',
  styleUrls: ['./payment-form.css'],
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
})
export class PaymentFormComponent {
  paymentForm: FormGroup;
  isLoading: boolean = false;

  constructor(
    private fb: FormBuilder,
    private razorpayService: RazorpayService,
    private orderService: OrderService
  ) {
    this.paymentForm = this.fb.group({
      amount: [0, [Validators.required, Validators.min(1)]],
      customerName: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      contactNumber: [
        '',
        [Validators.required, Validators.pattern('^[0-9]{10,}$')],
      ],
    });
  }

  async pay() {
    if (this.paymentForm.invalid) {
      this.paymentForm.markAllAsTouched();
      return;
    }
    this.isLoading = true;
    const { amount, customerName, email, contactNumber } =
      this.paymentForm.value;
    try {
      const order = await this.orderService
        .createOrder(amount, customerName, email, contactNumber)
        .subscribe({
          next: (order) => {
            console.log('Order created successfully:', order);
            this.razorpayService.initiateTransaction(
              amount,
              customerName,
              email,
              contactNumber,
              order.razorpayOrderId!,
              order.id
            );
          },
          error: (error) => {
            console.error('Error creating order:', error);
            alert('Failed to create order. Please try again.');
          },
        });
    } catch (error) {
      alert('Payment failed or cancelled.');
    } finally {
      this.isLoading = false;
    }
  }

  get amount() {
    return this.paymentForm.get('amount');
  }
  get customerName() {
    return this.paymentForm.get('customerName');
  }
  get email() {
    return this.paymentForm.get('email');
  }
  get contactNumber() {
    return this.paymentForm.get('contactNumber');
  }
}
