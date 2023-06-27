import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../common/translate.share.module';
import { WorkLoadMatrixComponent } from './work-load-matrix.component';
import { WorkLoadMatrixRoutingModule } from './work-load-matrix-routing.module';

 

@NgModule({
  declarations: [WorkLoadMatrixComponent],
  imports: [
    CommonModule,
    WorkLoadMatrixRoutingModule,
    SharedModule,
    FormsModule,
    NgbModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
   TranslateSharedModule.forRoot()
  ]
})
export class WorkLoadMatrixModule { 

}
