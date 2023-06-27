import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EmailConfigurationSummaryRoutingModule } from './email-configuration-summary-routing.module';
import { EmailConfigurationSummaryComponent } from './email-configuration-summary.component';
import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateSharedModule } from '../../common/translate.share.module';
import { ValidationPopupComponent } from '../../validation-popup/validation-popup.component';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
  declarations: [EmailConfigurationSummaryComponent],
  entryComponents:[ValidationPopupComponent],
  imports: [
    CommonModule,
    EmailConfigurationSummaryRoutingModule,
    SharedModule,
    FormsModule,
    NgSelectModule,
    TranslateSharedModule.forRoot(),
    NgbPaginationModule
  ]
})
export class EmailConfigurationSummaryModule { 
}
