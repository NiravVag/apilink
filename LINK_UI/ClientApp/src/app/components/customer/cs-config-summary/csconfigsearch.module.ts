import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CsconfigsearchRoutingModule } from './csconfigsearch-routing.module';
import { CSConfigSummaryComponent } from './cs-config-summary.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';
 

@NgModule({
  declarations: [
    CSConfigSummaryComponent
  ],
  imports: [
    CommonModule,
    CsconfigsearchRoutingModule,
    SharedModule,
    FormsModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class CsconfigsearchModule { 

}
