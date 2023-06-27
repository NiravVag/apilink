import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CustomereditRoutingModule } from './customeredit-routing.module';
import { EditCustomerComponent } from './edit-customer.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';
 
@NgModule({
  declarations: [EditCustomerComponent],
  imports: [
    CommonModule,
    CustomereditRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class CustomereditModule { 

}
