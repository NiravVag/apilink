import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { QcdashboardComponent } from './qcdashboard.component';
import { QcdashboardRoutingModule } from './qcdashboard-routing.module';
import { SharedModule } from '../../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule,NgbPopoverModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateSharedModule } from 'src/app/components/common/translate.share.module';

@NgModule({
  declarations: [QcdashboardComponent],
  imports: [
    CommonModule,
    QcdashboardRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
    NgbPopoverModule,
    TranslateSharedModule.forRoot()
  ]
})
export class QcdashboardModule { 

}
