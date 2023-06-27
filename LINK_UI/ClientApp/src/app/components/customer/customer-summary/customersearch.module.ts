import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CustomersearchRoutingModule } from './customersearch-routing.module';
import { CustomerSummaryComponent } from './customersummary.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';
 
@NgModule({
  declarations: [CustomerSummaryComponent],
  imports: [
    CommonModule,
    CustomersearchRoutingModule,
    SharedModule,
    FormsModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class CustomersearchModule {

 }
