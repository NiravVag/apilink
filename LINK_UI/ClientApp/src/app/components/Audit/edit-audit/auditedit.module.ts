import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuditeditRoutingModule } from './auditedit-routing.module';
import { EditAuditComponent } from './edit-audit.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';
 
@NgModule({
  declarations: [EditAuditComponent],
  imports: [
    CommonModule,
    AuditeditRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class AuditeditModule {
  
 }
