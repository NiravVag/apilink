import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuditDashboardRoutingModule } from './audit-dashboard-routing.module';
import { AuditDashboardComponent } from './audit-dashboard.component';
import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateSharedModule } from '../../common/translate.share.module';


@NgModule({
  declarations: [AuditDashboardComponent],
  imports: [
    CommonModule,
    AuditDashboardRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class AuditDashboardModule { }
