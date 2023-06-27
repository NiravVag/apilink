import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DefectDashboardRoutingModule } from './defect-dashboard-routing.module';
import { DefectDashboardComponent } from './defect-dashboard.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { HttpClient } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { TranslateModule, TranslateLoader, TranslateService } from '@ngx-translate/core';
import { NgbPaginationModule , NgbDatepickerModule} from '@ng-bootstrap/ng-bootstrap';
import { TranslateSharedModule } from '../../common/translate.share.module';
import { NgxGalleryModule } from 'ngx-gallery-9';

// AoT requires an exported function for factories
//export function HttpLoaderFactory(httpClient: HttpClient) {
//  var loader = new TranslateHttpLoader(httpClient, './assets/i18n/');
//  return loader;
//}


@NgModule({
  declarations: [DefectDashboardComponent],
  imports: [
    CommonModule,
    DefectDashboardRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgSelectModule,
    NgbPaginationModule,
    TranslateSharedModule.forRoot(),
    NgxGalleryModule
  ]
})
export class DefectDashboardModule { 

}
