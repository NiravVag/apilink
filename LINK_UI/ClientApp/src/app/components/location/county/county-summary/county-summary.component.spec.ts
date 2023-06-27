import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { CountySummaryComponent } from './county-summary.component';

describe('CountySummaryComponent', () => {
  let component: CountySummaryComponent;
  let fixture: ComponentFixture<CountySummaryComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ CountySummaryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CountySummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
