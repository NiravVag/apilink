import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EditCustomerCheckPointRoutingModule } from './edit-customer-checkpoint-routing.module';
import { EditCustomerCheckpointComponent } from './edit-customer-checkpoint.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';
 

@NgModule({
  declarations: [EditCustomerCheckpointComponent],
  imports: [
    CommonModule,
    EditCustomerCheckPointRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class EditCustomerCheckPointModule { 

}
