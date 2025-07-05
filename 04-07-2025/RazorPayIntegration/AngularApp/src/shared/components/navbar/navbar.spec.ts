import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Navbar } from './navbar';
import { ActivatedRoute } from '@angular/router';
import { of } from 'rxjs'; 

describe('Navbar', () => {
  let component: Navbar;
  let fixture: ComponentFixture<Navbar>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Navbar],
      providers: [
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

    fixture = TestBed.createComponent(Navbar);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
