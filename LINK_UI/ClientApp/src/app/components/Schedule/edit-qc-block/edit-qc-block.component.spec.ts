import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { EditQcBlockComponent } from './edit-qc-block.component';

describe('EditQcBlockComponent', () => {
  let component: EditQcBlockComponent;
  let fixture: ComponentFixture<EditQcBlockComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ EditQcBlockComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditQcBlockComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
