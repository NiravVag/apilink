import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule,NgbPopoverModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateSharedModule } from 'src/app/components/common/translate.share.module';
import { SupFactDashboardComponent } from './sup-fact-dashboard.component';
import { SupFactDashboardRoutingModule } from './sup-fact-dashboard-routing.module';

@NgModule({
  declarations: [SupFactDashboardComponent],
  imports: [
    CommonModule,
    SupFactDashboardRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
    NgbPopoverModule,
    TranslateSharedModule.forRoot()
  ]
})
export class SupFactDashboardModule { 

}
