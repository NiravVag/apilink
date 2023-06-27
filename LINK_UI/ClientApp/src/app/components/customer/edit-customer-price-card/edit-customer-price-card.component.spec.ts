import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { EditCustomerPriceCardComponent } from './edit-customer-price-card.component';

describe('EditCustomerPriceCardComponent', () => {
  let component: EditCustomerPriceCardComponent;
  let fixture: ComponentFixture<EditCustomerPriceCardComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ EditCustomerPriceCardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditCustomerPriceCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
