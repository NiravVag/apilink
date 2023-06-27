import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { QuantitativedashboardComponent } from './quantitativedashboard.component';
import { QuantitativedashboardRoutingModule } from './quantitativedashboard-routing.module';
import { SharedModule } from '../../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from 'src/app/components/common/translate.share.module';
 

@NgModule({
  declarations: [QuantitativedashboardComponent],
  imports: [
    CommonModule,
    QuantitativedashboardRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class QuantitativedashboardModule { 

}
