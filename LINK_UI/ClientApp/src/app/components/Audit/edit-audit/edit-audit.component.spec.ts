import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { EditAuditComponent } from './edit-audit.component';

describe('EditAuditComponent', () => {
  let component: EditAuditComponent;
  let fixture: ComponentFixture<EditAuditComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ EditAuditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditAuditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
