import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { CountrySummaryComponent } from './country-summary.component';

describe('CountrySummaryComponent', () => {
  let component: CountrySummaryComponent;
  let fixture: ComponentFixture<CountrySummaryComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ CountrySummaryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CountrySummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
