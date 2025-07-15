import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QuizSubmissionsPage } from './quiz-submissions-page';

describe('QuizSubmissionsPage', () => {
  let component: QuizSubmissionsPage;
  let fixture: ComponentFixture<QuizSubmissionsPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [QuizSubmissionsPage],
    }).compileComponents();

    fixture = TestBed.createComponent(QuizSubmissionsPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
