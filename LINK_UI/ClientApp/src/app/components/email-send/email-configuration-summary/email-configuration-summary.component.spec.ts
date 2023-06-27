import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { EmailConfigurationSummaryComponent } from './email-configuration-summary.component';

describe('EmailConfigurationSummaryComponent', () => {
  let component: EmailConfigurationSummaryComponent;
  let fixture: ComponentFixture<EmailConfigurationSummaryComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ EmailConfigurationSummaryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmailConfigurationSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
