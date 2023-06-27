import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManualInvoiceSummaryComponent } from './manual-invoice-summary.component';

describe('ManualInvoiceSummaryComponent', () => {
  let component: ManualInvoiceSummaryComponent;
  let fixture: ComponentFixture<ManualInvoiceSummaryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ManualInvoiceSummaryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ManualInvoiceSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
