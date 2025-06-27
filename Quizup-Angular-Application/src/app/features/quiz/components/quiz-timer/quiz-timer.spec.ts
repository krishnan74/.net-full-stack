import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QuizTimerComponent } from './quiz-timer';

describe('QuizTimer', () => {
  let component: QuizTimerComponent;
  let fixture: ComponentFixture<QuizTimerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [QuizTimerComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(QuizTimerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
