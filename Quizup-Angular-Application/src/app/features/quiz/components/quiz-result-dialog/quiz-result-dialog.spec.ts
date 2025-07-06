import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QuizResultDialog } from './quiz-result-dialog';

describe('QuizResultDialog', () => {
  let component: QuizResultDialog;
  let fixture: ComponentFixture<QuizResultDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [QuizResultDialog]
    })
    .compileComponents();

    fixture = TestBed.createComponent(QuizResultDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
