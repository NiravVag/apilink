import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuditcancelRoutingModule } from './auditcancel-routing.module';
import { CancelAuditComponent } from './cancel-audit.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';
 
@NgModule({
  declarations: [CancelAuditComponent],
  imports: [
    CommonModule,
    AuditcancelRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class AuditcancelModule { 

}
