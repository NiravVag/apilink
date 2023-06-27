import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { InspectioncertificateSummaryRoutingModule } from './inspectioncertificate-routing.module';
import { InspectioncertificateSummaryComponent } from './inspectioncertificate-summary.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';
 

@NgModule({
  declarations: [InspectioncertificateSummaryComponent],
  imports: [
    CommonModule,
    InspectioncertificateSummaryRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class InspectioncertificateSummaryModule {

 }
