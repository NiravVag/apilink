import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FoodAllowanceRoutingModule } from './food-allowance-routing.module';
import { FoodAllowanceComponent } from './food-allowance.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';

 

@NgModule({
  declarations: [FoodAllowanceComponent],
  imports: [
    CommonModule,
    FoodAllowanceRoutingModule,
    SharedModule,
    FormsModule,
    NgbModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
   TranslateSharedModule.forRoot()
  ]
})
export class FoodAllowanceModule { 

}
