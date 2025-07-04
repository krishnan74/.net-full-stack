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
import { API_BASE_URL } from './core/tokens/api-url.token';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(),
    {
      provide: API_BASE_URL,
      useValue: 'http://localhost:5182/api',
    },
    RazorpayService,
    OrderService,
    PaymentService,
  ],
};
