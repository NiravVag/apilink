import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HolidayRoutingModule } from './holiday-routing.module';
import { HolidaymasterComponent } from './holidaymaster.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import {  NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';
 

@NgModule({
  declarations: [HolidaymasterComponent],
  imports: [
    CommonModule,
    HolidayRoutingModule,
    SharedModule,
    FormsModule,
    NgbModule,
    NgSelectModule,
   TranslateSharedModule.forRoot()
  ]
})
export class HolidayModule { 

}
