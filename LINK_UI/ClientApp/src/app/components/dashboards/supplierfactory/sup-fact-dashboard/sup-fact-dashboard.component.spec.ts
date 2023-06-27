import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { SupFactDashboardComponent } from './sup-fact-dashboard.component';

describe('SupFactDashboardComponent', () => {
  let component: SupFactDashboardComponent;
  let fixture: ComponentFixture<SupFactDashboardComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ SupFactDashboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SupFactDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
