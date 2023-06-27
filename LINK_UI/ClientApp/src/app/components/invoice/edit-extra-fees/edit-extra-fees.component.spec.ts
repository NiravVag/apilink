import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { EditExtraFeesComponent } from './edit-extra-fees.component';

describe('EditExtraFeesComponent', () => {
  let component: EditExtraFeesComponent;
  let fixture: ComponentFixture<EditExtraFeesComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ EditExtraFeesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditExtraFeesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
