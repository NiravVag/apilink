import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SaleInvoiceSummaryRoutingModule } from './sale-invoice-summary-routing.module';
import { SaleInvoiceSummaryComponent } from './sale-invoice-summary.component';
import { NgbModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateSharedModule } from '../../common/translate.share.module';


@NgModule({
  declarations: [SaleInvoiceSummaryComponent],
  imports: [
    CommonModule,
    SaleInvoiceSummaryRoutingModule,
    NgbModule,
    SharedModule,
    FormsModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class SaleInvoiceSummaryModule { }
