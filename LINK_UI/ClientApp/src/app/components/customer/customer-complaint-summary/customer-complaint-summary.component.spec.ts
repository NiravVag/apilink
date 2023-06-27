import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerComplaintSummaryComponent } from './customer-complaint-summary.component';

describe('CustomerComplaintSummaryComponent', () => {
  let component: CustomerComplaintSummaryComponent;
  let fixture: ComponentFixture<CustomerComplaintSummaryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustomerComplaintSummaryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerComplaintSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
