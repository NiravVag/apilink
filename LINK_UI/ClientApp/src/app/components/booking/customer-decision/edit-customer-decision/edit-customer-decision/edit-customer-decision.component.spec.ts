import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditCustomerDecisionComponent } from './edit-customer-decision.component';

describe('EditCustomerDecisionComponent', () => {
  let component: EditCustomerDecisionComponent;
  let fixture: ComponentFixture<EditCustomerDecisionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditCustomerDecisionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditCustomerDecisionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
