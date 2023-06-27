import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { EditInvoiceBankComponent } from './edit-invoice-bank.component';

describe('EditInvoiceBankComponent', () => {
  let component: EditInvoiceBankComponent;
  let fixture: ComponentFixture<EditInvoiceBankComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ EditInvoiceBankComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditInvoiceBankComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
