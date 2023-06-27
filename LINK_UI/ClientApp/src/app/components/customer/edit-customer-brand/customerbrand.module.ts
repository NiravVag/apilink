import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CustomerbrandRoutingModule } from './customerbrand-routing.module';
import { EditCustomerBrandComponent } from './edit-customer-brand.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';
 

@NgModule({
  declarations: [EditCustomerBrandComponent],
  imports: [
    CommonModule,
    CustomerbrandRoutingModule,
    SharedModule,
    FormsModule,
    NgSelectModule,
   TranslateSharedModule.forRoot()
  ]
})
export class CustomerbrandModule {

 }
