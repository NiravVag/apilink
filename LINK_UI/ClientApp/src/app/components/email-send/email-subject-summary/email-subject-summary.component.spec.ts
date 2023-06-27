import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { EmailSubjectSummaryComponent } from './email-subject-summary.component';

describe('EmailSubjectSummaryComponent', () => {
  let component: EmailSubjectSummaryComponent;
  let fixture: ComponentFixture<EmailSubjectSummaryComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ EmailSubjectSummaryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmailSubjectSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
