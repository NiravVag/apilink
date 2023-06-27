import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProvinceRoutingModule } from './province-routing.module';
import { ProvinceSummaryComponent } from './province-summary/province-summary.component';
import { EditProvinceComponent } from './edit-province/edit-province.component';

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
  declarations: [ 
    ProvinceSummaryComponent,
    EditProvinceComponent],
  imports: [
    CommonModule,
    ProvinceRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class ProvinceModule {

 }
