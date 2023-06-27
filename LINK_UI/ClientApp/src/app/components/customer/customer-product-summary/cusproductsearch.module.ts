import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CusproductsearchRoutingModule } from './cusproductsearch-routing.module';
import { CustomerProductSummaryComponent } from './customerproductsummary.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';
 
@NgModule({
  declarations: [CustomerProductSummaryComponent],
  imports: [
    CommonModule,
    CusproductsearchRoutingModule,
    SharedModule,
    FormsModule,
    NgbPaginationModule,
    NgSelectModule,
   TranslateSharedModule.forRoot()
  ]
})
export class CusproductsearchModule {

 }
