import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import 'hammerjs';
import { CusproducteditRoutingModule } from './cusproductedit-routing.module';
import { EditCustomerProductComponent } from './editcustomerproduct.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { NgxGalleryModule } from 'ngx-gallery-9';
import { TranslateSharedModule } from '../../common/translate.share.module';
import { FileUploadComponent } from '../../file-upload/file-upload.component';
import { PipesModule } from '../../shared/pipe/pipes.module';

@NgModule({
  declarations: [EditCustomerProductComponent],
  exports: [EditCustomerProductComponent],
  entryComponents:[FileUploadComponent],
  imports: [
    CommonModule,
    CusproducteditRoutingModule,
    SharedModule,
    FormsModule,
    NgSelectModule,
    TranslateSharedModule.forRoot(),
    NgxGalleryModule,
    PipesModule
  ]
})
export class CusproducteditModule { 

}
