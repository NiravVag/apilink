import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CusservicesearchRoutingModule } from './cusservicesearch-routing.module';
import { CustomerServiceConfigurationComponent } from './customer-serviceconfiguration.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';
 

@NgModule({
  declarations: [CustomerServiceConfigurationComponent],
  imports: [
    CommonModule,
    CusservicesearchRoutingModule,
    SharedModule,
    FormsModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class CusservicesearchModule { 

}
