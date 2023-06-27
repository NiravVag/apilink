import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateSharedModule } from '../../common/translate.share.module';
import { InvoiceStatusRoutingModule } from './invoice-status-routing.module';
import { InvoiceStatusComponent } from './invoice-status.component';


@NgModule({
  declarations: [InvoiceStatusComponent],
  imports: [
    CommonModule,
    InvoiceStatusRoutingModule,
    NgbModule,
    SharedModule,
    FormsModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class InvoiceStatusModule 
 {
 }
