import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { EditEmailSubjectComponent } from './edit-email-subject.component';

describe('EditEmailSubjectComponent', () => {
  let component: EditEmailSubjectComponent;
  let fixture: ComponentFixture<EditEmailSubjectComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ EditEmailSubjectComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditEmailSubjectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
