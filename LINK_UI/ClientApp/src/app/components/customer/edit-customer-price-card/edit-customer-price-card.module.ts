import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EditCustomerPriceCardRoutingModule } from './edit-customer-price-card-routing.module';
import { EditCustomerPriceCardComponent } from './edit-customer-price-card.component';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';
 


@NgModule({
  declarations: [EditCustomerPriceCardComponent],
  imports: [
    CommonModule,
    EditCustomerPriceCardRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class EditCustomerPriceCardModule { 

}
