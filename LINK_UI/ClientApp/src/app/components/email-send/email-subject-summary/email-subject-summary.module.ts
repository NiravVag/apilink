import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EmailSubjectSummaryRoutingModule } from './email-subject-summary-routing.module';
import { EmailSubjectSummaryComponent } from './email-subject-summary.component';
import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateSharedModule } from '../../common/translate.share.module';
import { ValidationPopupComponent } from '../../validation-popup/validation-popup.component';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
  declarations: [EmailSubjectSummaryComponent],
  entryComponents:[ValidationPopupComponent],
  imports: [
    CommonModule,
    EmailSubjectSummaryRoutingModule,
    SharedModule,
    FormsModule,
    NgSelectModule,
    TranslateSharedModule.forRoot(),
    NgbPaginationModule
  ]
})
export class EmailSubjectSummaryModule { 

}
