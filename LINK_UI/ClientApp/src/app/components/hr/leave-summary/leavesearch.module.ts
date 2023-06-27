import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LeavesearchRoutingModule } from './leavesearch-routing.module';
import { LeaveSummaryComponent } from './leavesummary.component';


import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';

 
@NgModule({
  declarations: [LeaveSummaryComponent],
  imports: [
    CommonModule,
    LeavesearchRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class LeavesearchModule {

 }
