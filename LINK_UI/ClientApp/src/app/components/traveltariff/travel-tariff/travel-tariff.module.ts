import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';


import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { NgbDatepickerModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { TranslateSharedModule } from '../../common/translate.share.module';
import { TravelTariffComponent } from './travel-tariff.component';
import { TravelTariffRoutingModule } from './travel-tariff-routing.module';

 

@NgModule({
  declarations: [TravelTariffComponent],
  imports: [
    CommonModule,    
    TravelTariffRoutingModule,
    SharedModule,
    NgbDatepickerModule,
    FormsModule,
    NgSelectModule,
    NgbPaginationModule,
    TranslateSharedModule.forRoot()
  ]
})
export class TravelTariffModule { 

}
