import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LabeditRoutingModule } from './labedit-routing.module';
import { EditLabComponent } from './edit-lab.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
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
  declarations: [EditLabComponent],
  imports: [
    CommonModule,
    LabeditRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class LabeditModule {

 }
