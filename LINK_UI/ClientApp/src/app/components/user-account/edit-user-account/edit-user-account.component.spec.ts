import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { EditUserAccountComponent } from './edit-user-account.component';

describe('EditUserAccountComponent', () => {
  let component: EditUserAccountComponent;
  let fixture: ComponentFixture<EditUserAccountComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ EditUserAccountComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditUserAccountComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
