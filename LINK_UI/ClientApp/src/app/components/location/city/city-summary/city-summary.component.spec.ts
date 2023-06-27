import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { CitySummaryComponent } from './city-summary.component';

describe('CitySummaryComponent', () => {
  let component: CitySummaryComponent;
  let fixture: ComponentFixture<CitySummaryComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ CitySummaryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CitySummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
