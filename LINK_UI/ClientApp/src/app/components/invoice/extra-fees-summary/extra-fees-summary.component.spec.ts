import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { ExtraFeesSummaryComponent } from './extra-fees-summary.component';

describe('ExtraFeesSummaryComponent', () => {
  let component: ExtraFeesSummaryComponent;
  let fixture: ComponentFixture<ExtraFeesSummaryComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ ExtraFeesSummaryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExtraFeesSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
