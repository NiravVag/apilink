import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { StaffsearchRoutingModule } from './staffsearch-routing.module';
import { StaffSummaryComponent } from './staffsummary.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';
 
@NgModule({
  declarations: [StaffSummaryComponent],
  imports: [
    CommonModule,
    StaffsearchRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
TranslateSharedModule.forRoot()

  ]
})
export class StaffsearchModule {

 }
