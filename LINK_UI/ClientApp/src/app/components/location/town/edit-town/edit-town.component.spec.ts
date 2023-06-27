import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { EditTownComponent } from './edit-town.component';

describe('EditTownComponent', () => {
  let component: EditTownComponent;
  let fixture: ComponentFixture<EditTownComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ EditTownComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditTownComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
