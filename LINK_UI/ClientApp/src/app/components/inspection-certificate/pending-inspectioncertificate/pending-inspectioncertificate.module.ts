import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PendingInspectioncertificateRoutingModule } from './pending-inspectioncertificate-routing.module';
import { PendingInspectioncertificateComponent } from './pending-inspectioncertificate.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';
 

@NgModule({
  declarations: [PendingInspectioncertificateComponent],
  imports: [
    CommonModule,
    PendingInspectioncertificateRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class PendingInspectioncertificateModule {

 }
