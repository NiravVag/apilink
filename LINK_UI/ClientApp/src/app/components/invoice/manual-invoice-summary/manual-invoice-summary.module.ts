import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ManualInvoiceSummaryRoutingModule } from './manual-invoice-summary-routing.module';
import { ManualInvoiceSummaryComponent } from './manual-invoice-summary.component';
import { SharedModule } from 'src/app/components/shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateSharedModule } from 'src/app/components/common/translate.share.module';


@NgModule({
  declarations: [
    ManualInvoiceSummaryComponent    
  ],
  imports: [
    CommonModule,
    ManualInvoiceSummaryRoutingModule,
    SharedModule,
    NgbModule,
    FormsModule,
    NgSelectModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    TranslateSharedModule.forRoot()
  ]
})
export class ManualInvoiceSummaryModule { }
