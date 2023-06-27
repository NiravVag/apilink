import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DefectParetoRoutingModule } from './defect-pareto-routing.module';
import { DefectParetoComponent } from './defect-pareto.component';
import { SharedModule } from '../../shared/shared.module';
import { NgbDatepickerModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateSharedModule } from '../../common/translate.share.module';
import { NgxGalleryModule } from 'ngx-gallery-9';


@NgModule({
  declarations: [DefectParetoComponent],
  imports: [
    CommonModule,
    DefectParetoRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgSelectModule,
    NgbPaginationModule,
    TranslateSharedModule.forRoot(),
    NgxGalleryModule
  ]
})
export class DefectParetoModule { }
