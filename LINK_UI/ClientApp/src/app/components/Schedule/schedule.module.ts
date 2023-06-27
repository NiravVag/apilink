import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ScheduleRoutingModule } from './schedule-routing.module';
import { ScheduleSummaryComponent } from './schedule-summary/schedule-summary.component';
import { ScheduleAllocationComponent } from './schedule-allocation/schedule-allocation.component';

import { SharedModule } from '../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { HttpClient } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { TranslateModule, TranslateLoader, TranslateService } from '@ngx-translate/core';
import { QcAvailabilityComponent } from './qc-availability/qc-availability.component';
import { TranslateSharedModule } from '../common/translate.share.module';
import { EditQcBlockComponent } from './edit-qc-block/edit-qc-block.component';
import { ValidationPopupComponent } from '../validation-popup/validation-popup.component';
import { QcBlockSummaryComponent } from './qc-block-summary/qc-block-summary.component';
import { PipesModule } from '../shared/pipe/pipes.module';

// AoT requires an exported function for factories
//export function HttpLoaderFactory(httpClient: HttpClient) {
//  var loader = new TranslateHttpLoader(httpClient, './assets/i18n/');
//  return loader;
//}

@NgModule({
  declarations: [ScheduleSummaryComponent, ScheduleAllocationComponent, QcAvailabilityComponent, EditQcBlockComponent, QcBlockSummaryComponent],
  entryComponents:[ValidationPopupComponent],
  imports: [
    NgbModule,
    CommonModule,
    ScheduleRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgSelectModule,
    NgbPaginationModule,
    TranslateSharedModule.forRoot(),
    PipesModule
  ]
})
export class ScheduleModule {

 }
