import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CsdashboardComponent } from './csdashboard.component';
import { CsdashboardRoutingModule } from './csdashboard-routing.module';
import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from 'src/app/components/common/translate.share.module';
import { PipesModule } from '../../shared/pipe/pipes.module';
 

@NgModule({
  declarations: [CsdashboardComponent],
  imports: [
    CommonModule,
    PipesModule,
    CsdashboardRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class CsdashboardModule { 

}
