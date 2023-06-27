import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { EditInvoiceSendComponent } from './edit-invoice-send.component';


describe('EditEmailSendComponent', () => {
  let component: EditInvoiceSendComponent;
  let fixture: ComponentFixture<EditInvoiceSendComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ EditInvoiceSendComponent ]
    }) 
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditInvoiceSendComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
