import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { AuditCusReportComponent } from './audit-cus-report.component';

describe('AuditCusReportComponent', () => {
  let component: AuditCusReportComponent;
  let fixture: ComponentFixture<AuditCusReportComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ AuditCusReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AuditCusReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
