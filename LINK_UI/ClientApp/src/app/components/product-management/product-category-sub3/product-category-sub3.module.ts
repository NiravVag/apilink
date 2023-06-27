import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';
import { ProductCategorySub3Component } from './product-category-sub3.component';
import { ProductCategorySub3RoutingModule } from './product-category-sub3-routing.module';

 

@NgModule({
  declarations: [ProductCategorySub3Component],
  imports: [
    CommonModule,
    ProductCategorySub3RoutingModule,
    SharedModule,
    FormsModule,
    NgbModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
   TranslateSharedModule.forRoot()
  ]
})
export class ProductCategorySub3Module { 

}
