import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { BookingDetailRoutingModule } from './bookingdetail.routing.module';
import { BookingDetailComponent } from './booking-detail.component';
import { NgxGalleryModule } from 'ngx-gallery-9';
import { TranslateSharedModule } from '../../common/translate.share.module';
 

@NgModule({
  declarations: [BookingDetailComponent],
  imports: [
    CommonModule,
    BookingDetailRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot(),
    NgxGalleryModule
  ]
})
export class BookingDetailModule {
 
 }
