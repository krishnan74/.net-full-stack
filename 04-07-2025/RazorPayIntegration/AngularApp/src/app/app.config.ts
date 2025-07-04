import {
  ApplicationConfig,
  provideBrowserGlobalErrorListeners,
  provideZoneChangeDetection,
} from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient } from '@angular/common/http';
import { RazorpayService } from './features/razorpay/razorpay.service';
import { OrderService } from './features/order/services/order.service';
import { PaymentService } from './features/payment/services/payment.service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(),
    RazorpayService,
    OrderService,
    PaymentService,
  ],
};
