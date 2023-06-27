import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TcfSummaryRoutingModule } from './tcf-summary-routing.module';
import { TcfSummaryComponent } from './tcf-summary.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule, NgbPopoverModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { HttpClient } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { TranslateModule, TranslateLoader, TranslateService } from '@ngx-translate/core';
import { NgxGalleryModule } from 'ngx-gallery-9';
import { AppTranslateLoader } from '../../../_Services/common/app-translate-loader';


// AoT requires an exported function for factories
// export function HttpLoaderFactory(httpClient: HttpClient) {
//   var loader = new TranslateHttpLoader(httpClient, './assets/i18n/');
//   return loader;
// }


@NgModule({
  declarations: [TcfSummaryComponent],
  imports: [
    CommonModule,
    TcfSummaryRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
    NgbPopoverModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useClass: AppTranslateLoader,
        deps: [HttpClient]
      }
    }),
    NgxGalleryModule
  ]
})
export class TCFSummaryModule {
  constructor(public translate: TranslateService) {
    translate.addLangs(['en', 'fr']);
    translate.use('en');
  }
 }
