import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MandayUtilizationDashboardRoutingModule } from './manday-utilization-dashboard-routing.module';
import { MandayUtilizationDashboardComponent } from './manday-utilization-dashboard.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgbDatepickerModule} from '@ng-bootstrap/ng-bootstrap';
import { TranslateSharedModule } from '../../common/translate.share.module';

// AoT requires an exported function for factories
//export function HttpLoaderFactory(httpClient: HttpClient) {
//  var loader = new TranslateHttpLoader(httpClient, './assets/i18n/');
//  return loader;
//}


@NgModule({
  declarations: [MandayUtilizationDashboardComponent],
  imports: [
    CommonModule,
    MandayUtilizationDashboardRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class MandayUtilizationDashboardModule { 

}
