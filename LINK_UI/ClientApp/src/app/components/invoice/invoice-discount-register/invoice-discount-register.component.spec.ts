import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InvoiceDiscountRegisterComponent } from './invoice-discount-register.component';

describe('InvoiceDiscountRegisterComponent', () => {
  let component: InvoiceDiscountRegisterComponent;
  let fixture: ComponentFixture<InvoiceDiscountRegisterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InvoiceDiscountRegisterComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InvoiceDiscountRegisterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
