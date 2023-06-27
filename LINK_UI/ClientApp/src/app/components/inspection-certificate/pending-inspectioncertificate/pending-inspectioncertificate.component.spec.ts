import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { PendingInspectioncertificateComponent } from './pending-inspectioncertificate.component';

describe('PendingInspectioncertificateComponent', () => {
  let component: PendingInspectioncertificateComponent;
  let fixture: ComponentFixture<PendingInspectioncertificateComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ PendingInspectioncertificateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PendingInspectioncertificateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
