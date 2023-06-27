import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { CusdashboardComponent } from './cusdashboard.component';

describe('CusdashboardComponent', () => {
  let component: CusdashboardComponent;
  let fixture: ComponentFixture<CusdashboardComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ CusdashboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CusdashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
