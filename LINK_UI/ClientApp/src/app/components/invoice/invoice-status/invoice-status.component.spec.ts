import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { InvoiceStatusComponent } from './invoice-status.component';

describe('InvoiceSummaryComponent', () => {
  let component: InvoiceStatusComponent;
  let fixture: ComponentFixture<InvoiceStatusComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ InvoiceStatusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InvoiceStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
