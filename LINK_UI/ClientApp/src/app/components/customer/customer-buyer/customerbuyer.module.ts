import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CustomerbuyerRoutingModule } from './customerbuyer-routing.module';
import { CustomerBuyerComponent } from './customer-buyer.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';
 
@NgModule({
  declarations: [CustomerBuyerComponent],
  imports: [
    CommonModule,
    CustomerbuyerRoutingModule,
    SharedModule,
    FormsModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class CustomerbuyerModule { 

}
