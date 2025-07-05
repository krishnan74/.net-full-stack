import { Routes } from '@angular/router';
import { PaymentFormComponent } from './features/payment/payment-form/payment-form';
import { OrdersList } from './features/order/components/orders-list/orders-list';
import { PaymentsList } from './features/payment/components/payments-list/payments-list';

export const routes: Routes = [
    {
        path: 'pay',
        component: PaymentFormComponent
    }
    ,{
        path: 'orders',
        component: OrdersList
    },
    {
        path: 'payments',
        component: PaymentsList
    }
];
