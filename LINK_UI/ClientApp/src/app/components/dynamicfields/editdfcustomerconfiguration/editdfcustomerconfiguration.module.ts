import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EditDfCustomerConfigurationRoutingModule } from './editdfcustomerconfiguration.routing.module';
import { EditDfCustomerConfigurationComponent } from './editdfcustomerconfiguration.component';
import {DynamicFormComponent} from '../dynamic-form/dynamicform.component'
import { ReactiveFormsModule }          from '@angular/forms';


import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';
 

@NgModule({
  declarations: [EditDfCustomerConfigurationComponent],
  imports: [
    CommonModule,
    EditDfCustomerConfigurationRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgSelectModule,
    ReactiveFormsModule,
    TranslateSharedModule.forRoot()
  ]
})
export class DFCustomerConfigurationModule { 

}
