import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { InvoiceBankComponent } from './invoice-bank.component';

describe('InvoiceBankComponent', () => {
  let component: InvoiceBankComponent;
  let fixture: ComponentFixture<InvoiceBankComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ InvoiceBankComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InvoiceBankComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
