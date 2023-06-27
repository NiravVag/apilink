import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { NgbDatepickerModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { TranslateSharedModule } from '../../common/translate.share.module';
import { EditTravelTariffRoutingModule } from './edit-travel-tariff-routing.module';
import { EditTravelTariffComponent } from './edit-travel-tariff.component';


 

@NgModule({
  declarations: [EditTravelTariffComponent],
  imports: [
    CommonModule,
    EditTravelTariffRoutingModule ,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgSelectModule,
    NgbPaginationModule,
    TranslateSharedModule.forRoot()
  ]
})
export class EditTravelTariffModule 
{ 

}
