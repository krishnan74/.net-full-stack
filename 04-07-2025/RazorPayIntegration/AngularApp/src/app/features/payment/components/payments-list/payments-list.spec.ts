import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PaymentsList } from './payments-list';
import { API_BASE_URL } from '../../../../core/tokens/api-url.token';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { PaymentService } from '../../services/payment.service';

describe('PaymentsList', () => {
  let component: PaymentsList;
  let fixture: ComponentFixture<PaymentsList>;
  let httpMock: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PaymentsList, HttpClientTestingModule],
      providers: [
        PaymentService,
        { provide: API_BASE_URL, useValue: 'http://localhost:5182/api' },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(PaymentsList);
    component = fixture.componentInstance;
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch payments on initialization', () => {
    const mockPayments = [
      { id: 'payment_1', amount: 100, status: 'success' },
      { id: 'payment_2', amount: 200, status: 'failed' },
    ];

    component.ngOnInit();
    const req = httpMock.expectOne('http://localhost:5182/api/Payments');
    expect(req.request.method).toBe('GET');
    req.flush(mockPayments);

    expect(component.payments).toEqual(mockPayments);
  });


});
