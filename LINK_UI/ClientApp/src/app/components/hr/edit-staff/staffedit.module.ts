import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { StaffeditRoutingModule } from './staffedit-routing.module';
import { EditStaffComponent } from './edit-staff.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';
 


@NgModule({
  declarations:  [EditStaffComponent],
  imports: [
    CommonModule,
    StaffeditRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class StaffeditModule { 

}
