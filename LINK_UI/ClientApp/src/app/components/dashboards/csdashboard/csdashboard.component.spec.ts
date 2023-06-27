import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CsdashboardComponent } from './csdashboard.component';

describe('CsdashboardComponent', () => {
  let component: CsdashboardComponent;
  let fixture: ComponentFixture<CsdashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CsdashboardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CsdashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
