import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { ProvinceSummaryComponent } from './province-summary.component';

describe('ProvinceSummaryComponent', () => {
  let component: ProvinceSummaryComponent;
  let fixture: ComponentFixture<ProvinceSummaryComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ ProvinceSummaryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProvinceSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
