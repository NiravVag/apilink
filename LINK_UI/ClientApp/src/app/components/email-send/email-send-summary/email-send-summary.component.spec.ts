import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { EmailSendSummaryComponent } from './email-send-summary.component';

describe('EmailSendSummaryComponent', () => {
  let component: EmailSendSummaryComponent;
  let fixture: ComponentFixture<EmailSendSummaryComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ EmailSendSummaryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmailSendSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
