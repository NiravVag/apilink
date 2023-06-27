import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { DefectDashboardComponent } from './defect-dashboard.component';

describe('DefectDashboardComponent', () => {
  let component: DefectDashboardComponent;
  let fixture: ComponentFixture<DefectDashboardComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ DefectDashboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DefectDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
