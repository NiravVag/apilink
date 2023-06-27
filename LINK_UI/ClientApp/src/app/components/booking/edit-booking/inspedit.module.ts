import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import 'hammerjs';
import { NgxGalleryModule } from 'ngx-gallery-9';
import { InspeditRoutingModule } from './inspedit-routing.module';
import { EditBookingComponent } from './edit-booking.component';
import { CusproducteditModule } from '../../customer/edit-customer-product/cusproductedit.module';
import { SupplierliteModule } from '../../supplier/edit-supplier-lite/supplierlite.module';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { GroupByPipe, CombinedPipe, NonCombinedPipe, UniquePipe } from '../../shared/groupby/group-by.pipe';

import { ReactiveFormsModule }          from '@angular/forms';
import { TranslateSharedModule } from '../../common/translate.share.module';
import { FileUploadComponent } from '../../file-upload/file-upload.component';
import { ValidationPopupComponent } from '../../validation-popup/validation-popup.component';
import { DateStandardFormatPipe } from '../../shared/pipe/date-standardformat.pipe';
import { PipesModule } from '../../shared/pipe/pipes.module';
import { PurchaseOrderliteModule } from '../../purchaseorder/upload-purchaseorder-lite/purchaseorder-lite.module';
 
@NgModule({
    declarations: [EditBookingComponent, GroupByPipe, CombinedPipe,UniquePipe,NonCombinedPipe, 
    DateStandardFormatPipe],
  entryComponents:[FileUploadComponent,ValidationPopupComponent],
  imports: [
    NgbModule,
    CommonModule,
    InspeditRoutingModule,
    CusproducteditModule,
    SupplierliteModule,
    PurchaseOrderliteModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgSelectModule,
    ReactiveFormsModule,
    TranslateSharedModule.forRoot(),
    NgxGalleryModule,
    PipesModule
  ]
})
export class InspeditModule {

 }
