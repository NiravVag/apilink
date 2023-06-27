import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { ScheduleAllocationComponent } from './schedule-allocation.component';

describe('ScheduleAllocationComponent', () => {
  let component: ScheduleAllocationComponent;
  let fixture: ComponentFixture<ScheduleAllocationComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ ScheduleAllocationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ScheduleAllocationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
