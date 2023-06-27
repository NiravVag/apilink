import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MandayDashboardRoutingModule } from './manday-dashboard-routing.module';
import { MandayDashboardComponent } from './manday-dashboard.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { HttpClient } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { TranslateModule, TranslateLoader, TranslateService } from '@ngx-translate/core';
import { NgbPaginationModule , NgbDatepickerModule} from '@ng-bootstrap/ng-bootstrap';
import { TranslateSharedModule } from '../../common/translate.share.module';

// AoT requires an exported function for factories
//export function HttpLoaderFactory(httpClient: HttpClient) {
//  var loader = new TranslateHttpLoader(httpClient, './assets/i18n/');
//  return loader;
//}


@NgModule({
  declarations: [MandayDashboardComponent],
  imports: [
    CommonModule,
    MandayDashboardRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgSelectModule,
    NgbPaginationModule,
    TranslateSharedModule.forRoot()
  ]
})
export class MandayDashboardModule { 

}
