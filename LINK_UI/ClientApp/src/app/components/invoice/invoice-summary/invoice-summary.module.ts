import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateSharedModule } from '../../common/translate.share.module';
import { InvoiceSummaryRoutingModule } from './invoice-summary-routing.module';
import { InvoiceSummaryComponent } from './invoice-summary.component';


@NgModule({
  declarations: [InvoiceSummaryComponent],
  imports: [
    CommonModule,
    InvoiceSummaryRoutingModule,
    NgbModule,
    SharedModule,
    FormsModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class InvoiceSummaryModule 
 {
 }
