import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { RoleRightConfigurationComponent } from './role-right-configuration.component';

describe('RoleRightConfigurationComponent', () => {
  let component: RoleRightConfigurationComponent;
  let fixture: ComponentFixture<RoleRightConfigurationComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ RoleRightConfigurationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RoleRightConfigurationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
