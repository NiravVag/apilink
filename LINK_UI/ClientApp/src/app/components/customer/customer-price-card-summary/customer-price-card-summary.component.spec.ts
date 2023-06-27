import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { CustomerPriceCardSummaryComponent } from './customer-price-card-summary.component';

describe('CustomerPriceCardSummaryComponent', () => {
  let component: CustomerPriceCardSummaryComponent;
  let fixture: ComponentFixture<CustomerPriceCardSummaryComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ CustomerPriceCardSummaryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerPriceCardSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
