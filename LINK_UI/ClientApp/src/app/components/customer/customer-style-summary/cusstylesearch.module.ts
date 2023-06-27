import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CusstylesearchRoutingModule } from './cusstylesearch-routing.module';
import { CustomerStyleSummaryComponent } from './customerstylesummary.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';
 
@NgModule({
  declarations: [CustomerStyleSummaryComponent],
  imports: [
    CommonModule,
    CusstylesearchRoutingModule,
    SharedModule,
    FormsModule,
    NgbPaginationModule,
    NgSelectModule,
   TranslateSharedModule.forRoot()
  ]
})
export class CusstylesearchModule {

 }
