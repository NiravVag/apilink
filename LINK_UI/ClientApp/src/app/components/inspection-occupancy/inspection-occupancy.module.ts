import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { InspectionOccupancyRoutingModule } from './inspection-occupancy-routing.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { DndModule } from 'ngx-drag-drop';
import { TranslateSharedModule } from '../common/translate.share.module';
import { SharedModule } from '../shared/shared.module';
import { InspectionOccupancyComponent } from './inspection-occupancy.component';


@NgModule({
  declarations: [InspectionOccupancyComponent],
  imports: [
    CommonModule,
    InspectionOccupancyRoutingModule,
    SharedModule,
    FormsModule,
    NgbPaginationModule,
    NgbDatepickerModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class InspectionOccupancyModule { }
