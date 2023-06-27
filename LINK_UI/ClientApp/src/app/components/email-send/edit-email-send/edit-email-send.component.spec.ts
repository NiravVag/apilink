import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { EditEmailSendComponent } from './edit-email-send.component';

describe('EditEmailSendComponent', () => {
  let component: EditEmailSendComponent;
  let fixture: ComponentFixture<EditEmailSendComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ EditEmailSendComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditEmailSendComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
