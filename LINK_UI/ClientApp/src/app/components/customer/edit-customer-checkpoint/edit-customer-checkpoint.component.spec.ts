import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { EditCustomerCheckpointComponent } from './edit-customer-checkpoint.component';

describe('EditCustomerCheckpointComponent', () => {
  let component: EditCustomerCheckpointComponent;
  let fixture: ComponentFixture<EditCustomerCheckpointComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ EditCustomerCheckpointComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditCustomerCheckpointComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
