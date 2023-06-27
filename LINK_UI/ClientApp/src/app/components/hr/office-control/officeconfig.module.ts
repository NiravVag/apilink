import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { OfficeconfigRoutingModule } from './officeconfig-routing.module';
import { OfficeControlComponent } from './officecontrol.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';
 
@NgModule({
  declarations: [OfficeControlComponent],
  imports: [
    CommonModule,
    OfficeconfigRoutingModule,
    SharedModule,
    FormsModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class OfficeconfigModule { 

}
