import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateSharedModule } from '../../common/translate.share.module';
import { InvoiceGenerateRoutingModule } from './invoice-generate.routing.module';
import { InvoiceGenerateComponent } from './invoice-generate.component';


@NgModule({
  declarations: [InvoiceGenerateComponent],
  imports: [
    CommonModule,
    InvoiceGenerateRoutingModule,
    NgbModule,
    SharedModule,
    FormsModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class InvoiceGenerateModule 
 {
 }
