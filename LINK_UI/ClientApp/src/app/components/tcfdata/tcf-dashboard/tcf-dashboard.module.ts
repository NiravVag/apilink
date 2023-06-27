import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TcfDashboardRoutingModule } from './tcf-dashboard.routing.module';
import { TcfDashboardComponent } from './tcf-dashboard.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { HttpClient } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { TranslateModule, TranslateLoader, TranslateService } from '@ngx-translate/core';
import { NgxGalleryModule } from 'ngx-gallery-9';
import { TranslateSharedModule } from '../../common/translate.share.module';
import { FileUploadComponent } from '../../file-upload/file-upload.component';

// AoT requires an exported function for factories
/* export function HttpLoaderFactory(httpClient: HttpClient) {
  var loader = new TranslateHttpLoader(httpClient, './assets/i18n/');
  return loader;
} */


@NgModule({
  declarations: [TcfDashboardComponent],
  entryComponents:[FileUploadComponent],
  imports: [
    CommonModule,
    TcfDashboardRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot(),
    /* TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    }), */
    NgxGalleryModule
  ]
})
export class TCFDashboardModule {
  constructor(public translate: TranslateService) {
    translate.addLangs(['en', 'fr']);
    translate.use('en');
  }
 }
