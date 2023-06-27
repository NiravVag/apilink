import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LeaverequestRoutingModule } from './leaverequest-routing.module';
import { LeaveRequestComponent } from './leave-request.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';

 

@NgModule({
  declarations: [LeaveRequestComponent],
  imports: [
    CommonModule,
    LeaverequestRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgSelectModule,
   TranslateSharedModule.forRoot()
  ]
})
export class LeaverequestModule {

 }
