import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { MandayDashboardComponent } from './manday-dashboard.component';

describe('MandayDashboardComponent', () => {
  let component: MandayDashboardComponent;
  let fixture: ComponentFixture<MandayDashboardComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ MandayDashboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MandayDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
