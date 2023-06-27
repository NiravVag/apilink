import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { QcBlockSummaryComponent } from './qc-block-summary.component';

describe('QcBlockSummaryComponent', () => {
  let component: QcBlockSummaryComponent;
  let fixture: ComponentFixture<QcBlockSummaryComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ QcBlockSummaryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QcBlockSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
