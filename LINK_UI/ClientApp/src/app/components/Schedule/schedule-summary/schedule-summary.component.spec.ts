import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { ScheduleSummaryComponent } from './schedule-summary.component';

describe('ScheduleSummaryComponent', () => {
  let component: ScheduleSummaryComponent;
  let fixture: ComponentFixture<ScheduleSummaryComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ ScheduleSummaryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ScheduleSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
