import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbDropdown, NgbDropdownMenu, NgbModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { FullbridgeSummaryComponent } from './fullbridge-summary.component';
import { FullbridgesummaryRoutingModule } from './fullbridge-summary.routing';
import { TranslateSharedModule } from '../../common/translate.share.module';
 

@NgModule({
  declarations: [FullbridgeSummaryComponent],
  imports: [
    CommonModule,
    FullbridgesummaryRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
    NgbModule,
    TranslateSharedModule.forRoot()
  ]
})
export class FullbridgesummaryModule {

 }
