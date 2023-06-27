import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { SharedModule } from './../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { InvoiceDiscountSummaryRoutingModule } from './invoice-discount-summary-routing.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InvoiceDiscountSummaryComponent } from './invoice-discount-summary.component';
import { TranslateSharedModule } from '../../common/translate.share.module';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';



@NgModule({
  declarations: [
    InvoiceDiscountSummaryComponent
  ],
  imports: [
    CommonModule,
    InvoiceDiscountSummaryRoutingModule,
    FormsModule,
    SharedModule,
    NgbPaginationModule,
    NgbDatepickerModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class InvoiceDiscountSummaryModule { }
