import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DmUserManagementSummaryComponent } from './dm-user-management-summary.component';

describe('DmUserManagementSummaryComponent', () => {
  let component: DmUserManagementSummaryComponent;
  let fixture: ComponentFixture<DmUserManagementSummaryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DmUserManagementSummaryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DmUserManagementSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
