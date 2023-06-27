import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateSharedModule } from '../../common/translate.share.module';
import { InvoiceBankRoutingModule } from './invoice-bank-routing.module';
import { InvoiceBankComponent } from './invoice-bank.component';


@NgModule({
  declarations: [InvoiceBankComponent],
  imports: [
    CommonModule,
    InvoiceBankRoutingModule,
    SharedModule,
    FormsModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class InvoiceBankModule 
 {
 }
