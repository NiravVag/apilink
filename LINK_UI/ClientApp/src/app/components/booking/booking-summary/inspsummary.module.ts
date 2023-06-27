import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { InspsummaryRoutingModule } from './inspsummary-routing.module';
import { BookingSummaryComponent } from './booking-summary.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { NgxGalleryModule } from 'ngx-gallery-9';
import { TranslateSharedModule } from '../../common/translate.share.module';
import { BodyDropdownDirective } from '../../directives/body-dropdown.directive';

@NgModule({
  declarations: [BookingSummaryComponent,BodyDropdownDirective],
  imports: [
    CommonModule,
    InspsummaryRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot(),
    NgxGalleryModule
  ]
})
export class InspsummaryModule {
 
 }
