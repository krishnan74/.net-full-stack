import { TestBed } from '@angular/core/testing';
import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { OrderService } from './order.service';
import { OrderModel } from '../models/order';

describe('OrderService', () => {
  let service: OrderService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [OrderService],
    });
    service = TestBed.inject(OrderService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should call createOrder and return an Observable', () => {
    const mockOrder: OrderModel = {
      id: 'order_123',
      customerName: 'Test',
      email: 'test@example.com',
      contactNumber: '1234567890',
    };
    service
      .createOrder(100, 'Test', 'test@example.com', '1234567890')
      .subscribe((order) => {
        expect(order).toEqual(mockOrder);
      });
    const req = httpMock.expectOne('http://localhost:5182/api/Orders');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual({
      amount: 100,
      customerName: 'Test',
      email: 'test@example.com',
      contactNumber: '1234567890',
    });
    req.flush(mockOrder);
  });
});
