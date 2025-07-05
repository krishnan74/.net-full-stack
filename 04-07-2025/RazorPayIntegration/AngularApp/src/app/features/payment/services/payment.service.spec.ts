import { TestBed } from '@angular/core/testing';
import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { PaymentService } from './payment.service';
import { API_BASE_URL } from '../../../core/tokens/api-url.token'; 

describe('PaymentService', () => {
  let service: PaymentService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        PaymentService,
        { provide: API_BASE_URL, useValue: 'http://localhost:5182/api' }, 
      ],
    });
    service = TestBed.inject(PaymentService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should call createPayment and return an Observable', () => {
    const mockResponse = { id: 'payment_123' };
    service
      .createPayment(100, 'INR', 'success', 'order_123', 'razorpay_123')
      .subscribe((res) => {
        expect(res).toEqual(mockResponse);
      });
    const req = httpMock.expectOne('http://localhost:5182/api/Payments');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual({
      amount: 100,
      currency: 'INR',
      status: 'success',
      orderId: 'order_123',
      razorpayPaymentId: 'razorpay_123',
    });
    req.flush(mockResponse);
  });
});
