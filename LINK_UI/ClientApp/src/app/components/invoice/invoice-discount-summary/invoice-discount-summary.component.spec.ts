import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InvoiceDiscountSummaryComponent } from './invoice-discount-summary.component';

describe('InvoiceDiscountSummaryComponent', () => {
  let component: InvoiceDiscountSummaryComponent;
  let fixture: ComponentFixture<InvoiceDiscountSummaryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InvoiceDiscountSummaryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InvoiceDiscountSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
