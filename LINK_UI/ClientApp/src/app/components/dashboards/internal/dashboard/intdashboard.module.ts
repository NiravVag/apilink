import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { IntdashboardRoutingModule } from './intdashboard-routing.module';
import { DashboardComponent } from './dashboard.component';
import { SharedModule } from '../../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from 'src/app/components/common/translate.share.module';
 
@NgModule({
  declarations: [DashboardComponent],
  imports: [
    CommonModule,
    IntdashboardRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class IntdashboardModule { 

}
