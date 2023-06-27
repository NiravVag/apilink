import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CusdashboardComponent } from './cusdashboard.component';
import { CusdashboardRoutingModule } from './cusdashboard-routing.module';
import { SharedModule } from '../../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from 'src/app/components/common/translate.share.module';
 

@NgModule({
  declarations: [CusdashboardComponent],
  imports: [
    CommonModule,
    CusdashboardRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class CusdashboardModule { 

}
