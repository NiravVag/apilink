import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SaleInvoiceSummaryComponent } from './sale-invoice-summary.component';

describe('SaleInvoiceSummaryComponent', () => {
  let component: SaleInvoiceSummaryComponent;
  let fixture: ComponentFixture<SaleInvoiceSummaryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SaleInvoiceSummaryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SaleInvoiceSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
