import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditManualInvoiceComponent } from './edit-manual-invoice.component';

describe('EditManualInvoiceComponent', () => {
  let component: EditManualInvoiceComponent;
  let fixture: ComponentFixture<EditManualInvoiceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditManualInvoiceComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditManualInvoiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
