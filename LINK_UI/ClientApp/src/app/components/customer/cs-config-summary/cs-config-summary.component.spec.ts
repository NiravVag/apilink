import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { CSConfigSummaryComponent } from './cs-config-summary.component';

describe('CSConfigSummaryComponent', () => {
  let component: CSConfigSummaryComponent;
  let fixture: ComponentFixture<CSConfigSummaryComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ CSConfigSummaryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CSConfigSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
