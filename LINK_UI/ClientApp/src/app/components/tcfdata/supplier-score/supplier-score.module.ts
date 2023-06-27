import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SupplierScoreRoutingModule } from './supplier-score-routing.module';
import { TranslateService } from '@ngx-translate/core';
import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateSharedModule } from '../../common/translate.share.module';
import { NgxGalleryModule } from 'ngx-gallery-9';
import { SupplierScoreComponent } from './supplier-score.component';


@NgModule({
  declarations: [SupplierScoreComponent],
  imports: [
    CommonModule,
    SupplierScoreRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot(),
    NgxGalleryModule
  ]
})
export class SupplierScoreModule {
  constructor(public translate: TranslateService) {
    translate.addLangs(['en', 'fr']);
    translate.use('en');
  }
}
