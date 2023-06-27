import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { PurchaseorderComponent } from './purchaseorder.component';

describe('PurchaseorderComponent', () => {
  let component: PurchaseorderComponent;
  let fixture: ComponentFixture<PurchaseorderComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ PurchaseorderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PurchaseorderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
