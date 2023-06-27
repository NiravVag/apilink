import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { QcdashboardComponent } from './qcdashboard.component';

describe('QcdashboardComponent', () => {
  let component: QcdashboardComponent;
  let fixture: ComponentFixture<QcdashboardComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ QcdashboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QcdashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
