import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { EditEmailConfigurationComponent } from './edit-email-configuration.component';

describe('EditEmailConfigurationComponent', () => {
  let component: EditEmailConfigurationComponent;
  let fixture: ComponentFixture<EditEmailConfigurationComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ EditEmailConfigurationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditEmailConfigurationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
