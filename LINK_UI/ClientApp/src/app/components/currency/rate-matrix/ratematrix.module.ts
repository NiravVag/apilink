import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RatematrixRoutingModule } from './ratematrix-routing.module';
import { RateMatrixComponent } from './ratematrix.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';
 

@NgModule({
  declarations: [
    RateMatrixComponent
  ],
  imports: [
    CommonModule,
    RatematrixRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class RatematrixModule {

 }
