import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EditInspectioncertificateRoutingModule } from './edit-inspectioncertificate-routing.module';
import { EditInspectioncertificateComponent } from './edit-inspectioncertificate.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';

 

@NgModule({
  declarations: [EditInspectioncertificateComponent],
  imports: [
    CommonModule,
    EditInspectioncertificateRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class EditInspectioncertificateModule {

 }
