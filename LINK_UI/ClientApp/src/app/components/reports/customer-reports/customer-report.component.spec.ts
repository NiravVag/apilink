import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { CustomerReportComponent } from './customer-report.component';

describe('CustomerReportComponent', () => {
  let component: CustomerReportComponent;
  let fixture: ComponentFixture<CustomerReportComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ CustomerReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
