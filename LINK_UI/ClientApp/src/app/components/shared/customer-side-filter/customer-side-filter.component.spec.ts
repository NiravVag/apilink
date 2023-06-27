import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { CustomerSideFilterComponent } from './customer-side-filter.component';

describe('CustomerSideFilterComponent', () => {
  let component: CustomerSideFilterComponent;
  let fixture: ComponentFixture<CustomerSideFilterComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ CustomerSideFilterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerSideFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
