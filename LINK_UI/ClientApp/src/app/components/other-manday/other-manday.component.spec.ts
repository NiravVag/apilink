import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtherMandayComponent } from './other-manday.component';

describe('OtherMandayComponent', () => {
  let component: OtherMandayComponent;
  let fixture: ComponentFixture<OtherMandayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OtherMandayComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OtherMandayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
