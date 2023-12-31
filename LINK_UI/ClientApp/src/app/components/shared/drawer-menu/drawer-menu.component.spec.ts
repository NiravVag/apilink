import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { DrawerMenuComponent } from './drawer-menu.component';

describe('DrawerMenuComponent', () => {
  let component: DrawerMenuComponent;
  let fixture: ComponentFixture<DrawerMenuComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ DrawerMenuComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DrawerMenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
