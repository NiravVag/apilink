import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { EditPurchaseorderComponent } from './edit-purchaseorder.component';

describe('EditPurchaseorderComponent', () => {
  let component: EditPurchaseorderComponent;
  let fixture: ComponentFixture<EditPurchaseorderComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ EditPurchaseorderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditPurchaseorderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
