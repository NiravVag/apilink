import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { LabSummaryComponent } from './lab-summary.component';

describe('LabSummaryComponent', () => {
  let component: LabSummaryComponent;
  let fixture: ComponentFixture<LabSummaryComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ LabSummaryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LabSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
