import { ComponentFixture, TestBed } from '@angular/core/testing';
import { InvoiceDataAccessComponent } from './invoice-data-access.component';


describe('InvoiceDataAccessComponent', () => {
  let component: InvoiceDataAccessComponent;
  let fixture: ComponentFixture<InvoiceDataAccessComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InvoiceDataAccessComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InvoiceDataAccessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
