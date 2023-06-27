import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PoeditRoutingModule } from './poedit-routing.module';
import { EditPurchaseorderComponent } from './edit-purchaseorder.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule,NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { HttpClient } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { TranslateModule, TranslateLoader, TranslateService } from '@ngx-translate/core';
import { CusproducteditModule } from '../../customer/edit-customer-product/cusproductedit.module';
import { TranslateSharedModule } from '../../common/translate.share.module';

// AoT requires an exported function for factories
//export function HttpLoaderFactory(httpClient: HttpClient) {
//  var loader = new TranslateHttpLoader(httpClient, './assets/i18n/');
//  return loader;
//}

@NgModule({
  declarations: [EditPurchaseorderComponent],
  imports: [
    CommonModule,
    PoeditRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
    CusproducteditModule,
    TranslateSharedModule.forRoot()
  ]
})
export class PoeditModule {

 }
