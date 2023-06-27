import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { InsppickingRoutingModule } from './insppicking-routing.module';
import { EditInspectionPickingComponent } from './edit-inspectionpicking.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';
 
@NgModule({
  declarations: [EditInspectionPickingComponent],
  imports: [
    CommonModule,
    InsppickingRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()

  ]
})
export class InsppickingModule {

 }
