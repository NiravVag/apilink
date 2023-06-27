import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { FullbridgeSummaryComponent } from './fullbridge-summary.component';

describe('FullbridgeSummaryComponent', () => {
  let component: FullbridgeSummaryComponent;
  let fixture: ComponentFixture<FullbridgeSummaryComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ FullbridgeSummaryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FullbridgeSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
