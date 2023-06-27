import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateSharedModule } from '../../common/translate.share.module';
import { ExtraFeesSummaryRoutingModule } from './extra-fees-summary-routing.module';
import { ExtraFeesSummaryComponent } from './extra-fees-summary.component';

@NgModule({
  declarations: [ExtraFeesSummaryComponent],
  imports: [
    CommonModule,
    ExtraFeesSummaryRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgSelectModule,
    NgbModule,
    NgbPaginationModule,
    TranslateSharedModule.forRoot()
  ]
})
export class ExtraFeesSummaryModule 
{ 
}
