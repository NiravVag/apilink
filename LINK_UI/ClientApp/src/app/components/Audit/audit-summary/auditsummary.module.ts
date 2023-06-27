import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuditsummaryRoutingModule } from './auditsummary-routing.module';
import { AuditSummaryComponent } from './audit-summary.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';
 
@NgModule({
  declarations: [AuditSummaryComponent],
  imports: [
    CommonModule,
    AuditsummaryRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class AuditsummaryModule { 

}
