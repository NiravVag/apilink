import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { CSConfigRegisterComponent } from './cs-config-register.component';

describe('CSConfigRegisterComponent', () => {
  let component: CSConfigRegisterComponent;
  let fixture: ComponentFixture<CSConfigRegisterComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ CSConfigRegisterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CSConfigRegisterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
