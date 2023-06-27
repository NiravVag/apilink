import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { OfficeSummaryComponent } from './office-summary.component';

describe('OfficeSummaryComponent', () => {
  let component: OfficeSummaryComponent;
  let fixture: ComponentFixture<OfficeSummaryComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ OfficeSummaryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OfficeSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
