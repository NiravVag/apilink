import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { InvoiceDataAccessRoutingModule } from './invoice-data-access-routing.module';
import { InvoiceDataAccessComponent } from './invoice-data-access.component';
import { SharedModule } from '../../shared/shared.module';
import { TranslateSharedModule } from '../../common/translate.share.module';

 

@NgModule({
  declarations: [InvoiceDataAccessComponent],
  imports: [
    CommonModule,
    InvoiceDataAccessRoutingModule,
    SharedModule,
    FormsModule,
    NgbModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
   TranslateSharedModule.forRoot()
  ]
})
export class InvoiceDataAccessModule { 

}
