import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CustomerDecisionComponent } from './customer-decision-summary/customer-decision.component';
import { CustomerDecisionRoutingModule } from './inspcustomerdecision-routing.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from 'src/app/components/common/translate.share.module';
import { SharedModule } from '../../shared/shared.module';
import { NgxGalleryModule } from 'ngx-gallery-9';
import { EditCustomerDecisionComponent } from './edit-customer-decision/edit-customer-decision/edit-customer-decision.component';
 

@NgModule({
  declarations: [CustomerDecisionComponent, EditCustomerDecisionComponent],
  imports: [
    CommonModule,
    CustomerDecisionRoutingModule,  
    SharedModule,
    FormsModule,
    NgbModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot(),
    NgxGalleryModule
  ]
})
export class CustomerDecisionModule { 

}
