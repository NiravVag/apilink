import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { CustomKpiComponent } from './custom-kpi.component';

describe('CustomKPIComponent', () => {
  let component: CustomKpiComponent;
  let fixture: ComponentFixture<CustomKpiComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ CustomKpiComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomKpiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
