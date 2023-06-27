import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { AuthenticationNavigationComponent } from './authentication-navigation.component';

describe('AuthenticationNavigationComponent', () => {
  let component: AuthenticationNavigationComponent;
  let fixture: ComponentFixture<AuthenticationNavigationComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ AuthenticationNavigationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AuthenticationNavigationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
