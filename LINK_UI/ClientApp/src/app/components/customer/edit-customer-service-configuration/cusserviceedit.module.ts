import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CusserviceeditRoutingModule } from './cusserviceedit-routing.module';
import { EditCustomerServiceConfigComponent } from './edit-customerserviceconfig.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';
 

@NgModule({
  declarations: [EditCustomerServiceConfigComponent],
  imports: [
    CommonModule,
    CusserviceeditRoutingModule,
    SharedModule,
    FormsModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class CusserviceeditModule { 

}
