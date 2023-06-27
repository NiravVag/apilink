import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateSharedModule } from '../../common/translate.share.module';
import { EditInvoiceBankComponent } from './edit-invoice-bank.component';
import { EditInvoiceBankRoutingModule } from './edit-invoice-bank-routing.module';

@NgModule({
  declarations: [EditInvoiceBankComponent],
  imports: [
    CommonModule,
    EditInvoiceBankRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class EditInvoiceBankModule 
{ 
}
