import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { EditSupplierLiteComponent } from './edit-supplier-lite.component';

describe('EditSupplierLiteComponent', () => {
  let component: EditSupplierLiteComponent;
  let fixture: ComponentFixture<EditSupplierLiteComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ EditSupplierLiteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditSupplierLiteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
