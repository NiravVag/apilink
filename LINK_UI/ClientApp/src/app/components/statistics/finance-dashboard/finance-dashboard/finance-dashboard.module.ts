import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FinanceDashboardRoutingModule } from './finance-dashboard-routing.module';
import { FinanceDashboardComponent } from './finance-dashboard.component';

import { SharedModule } from '../../../shared/shared.module'
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { HttpClient } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { TranslateModule, TranslateLoader, TranslateService } from '@ngx-translate/core';
import { NgbPaginationModule , NgbDatepickerModule} from '@ng-bootstrap/ng-bootstrap';
import { TranslateSharedModule } from '../../../common/translate.share.module';
import { NgxGalleryModule } from 'ngx-gallery-9';
import { PipesModule } from 'src/app/components/shared/pipe/pipes.module';


@NgModule({
  declarations: [FinanceDashboardComponent],
  imports: [
    CommonModule,
    PipesModule,
    FinanceDashboardRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgSelectModule,
    NgbPaginationModule,
    TranslateSharedModule.forRoot(),
    NgxGalleryModule
  ]
})
export class FinanceDashboardModule { 

}
