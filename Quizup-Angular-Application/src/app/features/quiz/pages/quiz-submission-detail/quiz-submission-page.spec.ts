import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QuizSubmissionPage } from './quiz-submission-page';

describe('QuizSubmissionPage', () => {
  let component: QuizSubmissionPage;
  let fixture: ComponentFixture<QuizSubmissionPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [QuizSubmissionPage],
    }).compileComponents();

    fixture = TestBed.createComponent(QuizSubmissionPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
