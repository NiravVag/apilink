import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EditCustomerComplaintRoutingModule } from './edit-customer-complaint-routing.module';
import { EditCustomerComplaintComponent } from './edit-customer-complaint.component';
import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateSharedModule } from '../../common/translate.share.module';

@NgModule({
  declarations: [EditCustomerComplaintComponent],
  imports: [
    CommonModule,
    EditCustomerComplaintRoutingModule,
    SharedModule, 
    FormsModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class EditCustomerComplaintModule { 
}
