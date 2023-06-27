import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { QuotationeditRoutingModule } from './quotationedit-routing.module';
import { EditQuotationComponent } from './edit-quotation.component';
import { QuotationContactComponent } from '../quotation-contact/quotation-contact.component';
import { QuotationSummaryComponent } from '../quotation-summary/quotationsummary.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { HttpClient } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { TranslateModule, TranslateLoader, TranslateService } from '@ngx-translate/core';
import { CustomButton } from '../../common/button.component';
import { NgxGalleryModule } from 'ngx-gallery-9';
import { TranslateSharedModule } from '../../common/translate.share.module';

// AoT requires an exported function for factories
//export function HttpLoaderFactory(httpClient: HttpClient) {
//  var loader = new TranslateHttpLoader(httpClient, './assets/i18n/');
//  return loader;
//}


@NgModule({
  declarations: [EditQuotationComponent, QuotationContactComponent, CustomButton, QuotationSummaryComponent],
  imports: [
    NgbModule,
    CommonModule,
    QuotationeditRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot(),
    NgxGalleryModule
  ]
})
export class QuotationeditModule { 

}
