import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { InspcancelRoutingModule } from './inspcancel-routing.module';
import { CancelBookingComponent } from './cancel-booking.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateSharedModule } from '../../common/translate.share.module';
 
@NgModule({
  declarations: [CancelBookingComponent],
  imports: [
    CommonModule,
    InspcancelRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class InspcancelModule { 
 
}
