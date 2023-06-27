import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TCFDocumentRoutingModule } from './tcf-document-routing.module';
import { TcfDocumentComponent } from './tcf-document.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgxGalleryModule } from 'ngx-gallery-9';
import { TranslateSharedModule } from '../../common/translate.share.module';

// // AoT requires an exported function for factories
// export function HttpLoaderFactory(httpClient: HttpClient) {
//   var loader = new TranslateHttpLoader(httpClient, './assets/i18n/');
//   return loader;
// }


@NgModule({
  declarations: [TcfDocumentComponent],
  imports: [
    CommonModule,
    TCFDocumentRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot(),
    NgxGalleryModule
  ]
})
export class TCFDocumentModule {
 
 }
