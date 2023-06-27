import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CuscontacteditRoutingModule } from './cuscontactedit-routing.module';
import { EditCustomerContactComponent } from './edit-customercontact.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';

 

@NgModule({
  declarations: [EditCustomerContactComponent],
  imports: [
    CommonModule,
    CuscontacteditRoutingModule,
    SharedModule,
    FormsModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class CuscontacteditModule { 

}
