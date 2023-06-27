import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { CancelAuditComponent } from './cancel-audit.component';

describe('CancelAuditComponent', () => {
  let component: CancelAuditComponent;
  let fixture: ComponentFixture<CancelAuditComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ CancelAuditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CancelAuditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
