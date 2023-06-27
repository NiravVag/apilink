import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CuscontactsearchRoutingModule } from './cuscontactsearch-routing.module';
import { CustomerContactSummaryComponent } from './customercontactsummary.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';
 

@NgModule({
  declarations: [CustomerContactSummaryComponent],
  imports: [
    CommonModule,
    CuscontactsearchRoutingModule,
    SharedModule,
    FormsModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class CuscontactsearchModule { 

}
