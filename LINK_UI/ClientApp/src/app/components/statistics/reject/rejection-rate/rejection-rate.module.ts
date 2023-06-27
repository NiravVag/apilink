import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RejectionRateRoutingModule } from './rejection-rate-routing.module';
import { RejectionRateComponent } from './rejection-rate.component';
import { SharedModule } from 'src/app/components/shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateSharedModule } from 'src/app/components/common/translate.share.module';
import { NgxGalleryModule } from 'ngx-gallery-9';

@NgModule({
  declarations: [RejectionRateComponent],
  imports: [
    CommonModule,
    RejectionRateRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot(),
    NgxGalleryModule
  ]
})
export class RejectionRateModule { }
