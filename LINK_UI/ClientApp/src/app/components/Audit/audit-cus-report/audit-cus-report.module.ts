import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuditCusReportRoutingModule } from './audit-cus-report-routing.module';
import { AuditCusReportComponent } from './audit-cus-report.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateSharedModule } from '../../common/translate.share.module';
 
@NgModule({
  declarations: [AuditCusReportComponent],
  imports: [
    CommonModule,
    AuditCusReportRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class AuditCusReportModule {
  
 }
