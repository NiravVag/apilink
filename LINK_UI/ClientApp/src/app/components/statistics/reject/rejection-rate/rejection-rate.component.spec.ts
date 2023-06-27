import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RejectionRateComponent } from './rejection-rate.component';

describe('RejectionRateComponent', () => {
  let component: RejectionRateComponent;
  let fixture: ComponentFixture<RejectionRateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RejectionRateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RejectionRateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
