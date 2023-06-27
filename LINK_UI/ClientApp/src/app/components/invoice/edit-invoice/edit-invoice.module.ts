import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateSharedModule } from '../../common/translate.share.module';
import { EditInvoiceRoutingModule } from './edit-invoice.routing.module';
import { EditInvoiceComponent } from './edit-invoice.component';


@NgModule({
  declarations: [EditInvoiceComponent],
  imports: [
    CommonModule,
    EditInvoiceRoutingModule,
    NgbModule,
    SharedModule,
    FormsModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class EditInvoiceModule 
 {
 }
