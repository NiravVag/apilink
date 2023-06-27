import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { QcAvailabilityComponent } from './qc-availability.component';

describe('QcAvailabilityComponent', () => {
  let component: QcAvailabilityComponent;
  let fixture: ComponentFixture<QcAvailabilityComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ QcAvailabilityComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QcAvailabilityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
