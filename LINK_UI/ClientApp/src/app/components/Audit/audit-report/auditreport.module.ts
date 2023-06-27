import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuditreportRoutingModule } from './auditreport-routing.module';
import { AuditReportComponent } from './audit-report.component';
import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateSharedModule } from '../../common/translate.share.module';
 
@NgModule({
  declarations: [AuditReportComponent],
  imports: [
    CommonModule,
    AuditreportRoutingModule ,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgSelectModule,
    TranslateSharedModule.forRoot() 
  ]
})
export class AuditreportModule { 

}
