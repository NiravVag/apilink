import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { MandayUtilizationDashboardComponent } from './manday-utilization-dashboard.component';

describe('MandayUtilizationDashboardComponent', () => {
  let component: MandayUtilizationDashboardComponent;
  let fixture: ComponentFixture<MandayUtilizationDashboardComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ MandayUtilizationDashboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MandayUtilizationDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
