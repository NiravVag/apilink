import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerDecisionComponent } from './customer-decision.component';

describe('CustomerDecisionComponent', () => {
  let component: CustomerDecisionComponent;
  let fixture: ComponentFixture<CustomerDecisionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CustomerDecisionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerDecisionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
