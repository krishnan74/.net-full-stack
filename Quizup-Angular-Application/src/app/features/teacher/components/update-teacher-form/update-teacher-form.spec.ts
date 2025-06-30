import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateTeacherForm } from './update-teacher-form';

describe('UpdateTeacherForm', () => {
  let component: UpdateTeacherForm;
  let fixture: ComponentFixture<UpdateTeacherForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UpdateTeacherForm]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdateTeacherForm);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
