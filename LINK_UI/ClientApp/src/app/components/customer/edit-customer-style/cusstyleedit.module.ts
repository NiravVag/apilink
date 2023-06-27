import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import 'hammerjs';
import { CusstyleeditRoutingModule } from './cusstyleedit-routing.module';
import { EditCustomerStyleComponent } from './editcustomerstyle.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { NgxGalleryModule } from 'ngx-gallery-9';
import { TranslateSharedModule } from '../../common/translate.share.module';
import { FileUploadComponent } from '../../file-upload/file-upload.component';
 

@NgModule({
  declarations: [EditCustomerStyleComponent],
  exports: [EditCustomerStyleComponent],
  entryComponents:[FileUploadComponent],
  imports: [
    CommonModule,
    CusstyleeditRoutingModule,
    SharedModule,
    FormsModule,
    NgSelectModule,
    TranslateSharedModule.forRoot(),
    NgxGalleryModule
  ]
})
export class CusstyleeditModule { 

}
