import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { QuantitativedashboardComponent } from './quantitativedashboard.component';

describe('QuantitativedashboardComponent', () => {
  let component: QuantitativedashboardComponent;
  let fixture: ComponentFixture<QuantitativedashboardComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ QuantitativedashboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QuantitativedashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
