import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EditManualInvoiceRoutingModule } from './edit-manual-invoice-routing.module';
import { EditManualInvoiceComponent } from './edit-manual-invoice.component';
import { SharedModule } from 'src/app/components/shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateSharedModule } from 'src/app/components/common/translate.share.module';


@NgModule({
  declarations: [
    EditManualInvoiceComponent    
  ],
  imports: [
    CommonModule,
    EditManualInvoiceRoutingModule,
    NgbModule,
    SharedModule,
    FormsModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class EditManualInvoiceModule { }
