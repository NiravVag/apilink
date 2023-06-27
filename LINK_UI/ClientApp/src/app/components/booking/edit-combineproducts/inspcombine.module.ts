import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { InspcombineRoutingModule } from './inspcombine-routing.module';
import { EditCombineOrdersComponent } from './edit-combineorders.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';
 
@NgModule({
  declarations: [EditCombineOrdersComponent],
  imports: [
    CommonModule,
    InspcombineRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class InspcombineModule { 

}
