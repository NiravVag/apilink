import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TravelMatrixRoutingModule } from './travel-matrix-routing.module';
import { TravelMatrixComponent } from './travel-matrix.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { TranslateSharedModule } from '../../common/translate.share.module';

 

@NgModule({
  declarations: [TravelMatrixComponent],
  imports: [
    CommonModule,
    TravelMatrixRoutingModule,
    SharedModule,
    FormsModule,
    NgSelectModule,
    NgbPaginationModule,
    TranslateSharedModule.forRoot()
  ]
})
export class TravelMatrixModule { 

}
