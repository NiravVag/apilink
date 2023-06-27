import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { TownSummaryComponent } from './town-summary.component';

describe('TownSummaryComponent', () => {
  let component: TownSummaryComponent;
  let fixture: ComponentFixture<TownSummaryComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ TownSummaryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TownSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
