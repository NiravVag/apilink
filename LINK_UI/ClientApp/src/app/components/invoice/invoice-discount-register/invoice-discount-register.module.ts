import { InvoiceDiscountRegisterComponent } from './invoice-discount-register.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InvoiceDiscountRegisterRoutingModule } from './invoice-discount-register-routing.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateSharedModule } from '../../common/translate.share.module';
import { SharedModule } from '../../shared/shared.module';



@NgModule({
  declarations: [InvoiceDiscountRegisterComponent],
  imports: [
    CommonModule,
    InvoiceDiscountRegisterRoutingModule,
    SharedModule,
    FormsModule,
    NgbPaginationModule,
    NgSelectModule,
    NgbDatepickerModule,
    TranslateSharedModule.forRoot()
  ]
})
export class InvoiceDiscountRegisterModule { }
