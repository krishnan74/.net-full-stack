import { TestBed } from '@angular/core/testing';
import { App } from './app';
import { RazorpayService } from './features/razorpay/razorpay.service';
import { PaymentService } from './features/payment/services/payment.service';
import { OrderService } from './features/order/services/order.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs'; 

describe('App', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [App, HttpClientTestingModule],
      providers: [
        RazorpayService,
        OrderService,
        PaymentService,
        {
          provide: ActivatedRoute,
          useValue: {
            params: of({}), 
            queryParams: of({}), 
            snapshot: {
              paramMap: {
                get: (key: string) => null, 
              },
            },
          },
        },
      ],
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(App);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it('should render title', () => {
    const fixture = TestBed.createComponent(App);
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('h1')?.textContent).toContain(
      'RazorPay Integration with Angular'
    );
  });
});
