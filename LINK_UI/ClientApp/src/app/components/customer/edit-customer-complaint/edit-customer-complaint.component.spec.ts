import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { EditCustomerComplaintComponent } from './edit-customer-complaint.component';

describe('EditCustomerComplaintComponent', () => {
  let component: EditCustomerComplaintComponent;
  let fixture: ComponentFixture<EditCustomerComplaintComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ EditCustomerComplaintComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditCustomerComplaintComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
