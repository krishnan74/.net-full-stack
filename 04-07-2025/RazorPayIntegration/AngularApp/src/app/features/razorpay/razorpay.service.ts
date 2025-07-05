import { Inject, inject, Injectable, model } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { PaymentService } from '../payment/services/payment.service';

declare var Razorpay: any;

@Injectable()
export class RazorpayService {
  constructor(
    private http: HttpClient,
    private paymentService: PaymentService
  ) {}

  initiateTransaction = async (
    amount: number,
    customerName: string,
    email: string,
    contactNumber: string,
    razorpayOrderId: string,
    orderId: string
  ): Promise<boolean> => {
    return new Promise((resolve, reject) => {
      console.log('Initiating transaction with Razorpay');
      console.log(
        `Amount: ${amount}, Customer Name: ${customerName}, Email: ${email}, Contact: ${contactNumber}, Order ID: ${orderId}`
      );
      var options = {
        key: environment.RAZOR_PAY_KEY_ID,
        name: 'Razorpay Integration',
        description: 'Test Transaction',
        amount: amount * 100,
        order_id: razorpayOrderId,
        currency: 'INR',
        handler: (response: any) => {
          console.log('Payment successful:', response);
          this.paymentService
            .createPayment(
              amount,
              'INR',
              'success',
              orderId,
              response.razorpay_payment_id
            )
            .subscribe({
              next: (paymentResponse) => {
                console.log('Payment recorded successfully:', paymentResponse);
                resolve(true);
              },
              error: (error) => {
                console.error('Error recording payment:', error);
                reject(false);
              },
            });
        },
        prefill: {
          name: customerName,
          email: email,
          contact: contactNumber,
        },
        method: {
          upi: true,
        },
        theme: {
          color: '#6c63ff',
        },
        modal: {
          ondismiss: () => {
            console.log('Payment modal dismissed');
          },
        },
      };

      const rzp = new Razorpay(options);
      rzp.on('payment.failed', (response: any) => {
        console.log(response.error);
        this.paymentService
          .createPayment(
            amount,
            'INR',
            'failed',
            orderId,
            response.razorpay_payment_id
          )
          .subscribe({
            next: (paymentResponse) => {
              console.log(
                'Failed payment recorded successfully:',
                paymentResponse
              );
              reject(false);
            },
            error: (error) => {
              console.error('Error recording failed payment:', error);
              reject(false);
            },
          });
      });
      rzp.open();
    });
  };
}
