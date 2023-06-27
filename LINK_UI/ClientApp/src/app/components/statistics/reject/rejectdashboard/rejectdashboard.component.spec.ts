import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { RejectdashboardComponent } from './rejectdashboard.component';

describe('RejectdashboardComponent', () => {
  let component: RejectdashboardComponent;
  let fixture: ComponentFixture<RejectdashboardComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ RejectdashboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RejectdashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
