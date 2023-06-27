import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CsconfigregRoutingModule } from './csconfigreg-routing.module';
import { CSConfigRegisterComponent } from './cs-config-register.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateSharedModule } from '../../common/translate.share.module';
 

@NgModule({
  declarations: [
    CSConfigRegisterComponent
  ],
  imports: [
    CommonModule,
    CsconfigregRoutingModule,
    SharedModule,
    FormsModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class CsconfigregModule { 

}
