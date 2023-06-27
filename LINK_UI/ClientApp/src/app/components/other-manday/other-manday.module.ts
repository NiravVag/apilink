import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../common/translate.share.module';
import { OtherMandayComponent } from './other-manday.component';
import { OtherMandayRoutingModule } from './other-manday-routing.module';

 

@NgModule({
  declarations: [OtherMandayComponent],
  imports: [
    CommonModule,
    OtherMandayRoutingModule,
    SharedModule,
    FormsModule,
    NgbModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
   TranslateSharedModule.forRoot()
  ]
})
export class OtherMandayModule { 

}
