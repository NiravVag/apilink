import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { Productsub2categoryRoutingModule } from './productsub2category-routing.module';
import { EditProductCategorySub2Component } from './edit-product-category-sub2/edit-product-category-sub2.component';
import { ProductCategorySub2SummaryComponent } from './product-category-sub2-summary/product-category-sub2-summary.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { HttpClient } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { TranslateModule, TranslateLoader, TranslateService } from '@ngx-translate/core';
import { TranslateSharedModule } from '../../common/translate.share.module';

// AoT requires an exported function for factories
//export function HttpLoaderFactory(httpClient: HttpClient) {
//  var loader = new TranslateHttpLoader(httpClient, './assets/i18n/');
//  return loader;
//}

@NgModule({
  declarations: [ EditProductCategorySub2Component,
    ProductCategorySub2SummaryComponent],
  imports: [
    CommonModule,
    Productsub2categoryRoutingModule,
    SharedModule,
    FormsModule,
    NgbPaginationModule,
    NgbDatepickerModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class Productsub2categoryModule {

 }
