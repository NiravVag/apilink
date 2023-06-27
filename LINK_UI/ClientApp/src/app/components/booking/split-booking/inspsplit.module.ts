import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { InspsplitRoutingModule } from './inspsplit-routing.module';
import { SplitBookingComponent } from './split-booking.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';
 

@NgModule({
  declarations: [SplitBookingComponent],
  imports: [
    CommonModule,
    InspsplitRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class InspsplitModule { 

}
