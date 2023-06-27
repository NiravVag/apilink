import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { UploadPurchaseorderComponent } from './upload-purchaseorder.component';

describe('UploadPurchaseorderComponent', () => {
  let component: UploadPurchaseorderComponent;
  let fixture: ComponentFixture<UploadPurchaseorderComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ UploadPurchaseorderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UploadPurchaseorderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
