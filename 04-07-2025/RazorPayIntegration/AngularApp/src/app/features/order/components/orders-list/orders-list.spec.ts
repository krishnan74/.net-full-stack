import { ComponentFixture, TestBed } from '@angular/core/testing';
import { OrdersList } from './orders-list';
import { OrderService } from '../../services/order.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { API_BASE_URL } from '../../../../core/tokens/api-url.token';

describe('OrdersList', () => {
  let component: OrdersList;
  let fixture: ComponentFixture<OrdersList>;
  let httpMock: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OrdersList, HttpClientTestingModule],
      providers: [
        OrderService,
        { provide: API_BASE_URL, useValue: 'http://localhost:5182/api' },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(OrdersList);
    component = fixture.componentInstance;
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify(); 
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch orders on initialization', () => {
    const mockOrders = [
      { id: 'order_1', customerName: 'John Doe', amount: 100 },
      { id: 'order_2', customerName: 'Jane Doe', amount: 200 },
    ];

    component.ngOnInit();

    const req = httpMock.expectOne('http://localhost:5182/api/Orders');
    expect(req.request.method).toBe('GET');
    req.flush(mockOrders);

    expect(component.orders).toEqual(mockOrders);
  });


});
