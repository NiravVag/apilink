import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { ReportSummaryComponent } from './report-summary.component';
import { ReportsummaryRoutingModule } from './reportsummmary-routing';
import { TranslateSharedModule } from '../../common/translate.share.module';
 

@NgModule({
  declarations: [ReportSummaryComponent],
  imports: [
    NgbModule,
    CommonModule,
    ReportsummaryRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class ReportsummaryModule {

 }
