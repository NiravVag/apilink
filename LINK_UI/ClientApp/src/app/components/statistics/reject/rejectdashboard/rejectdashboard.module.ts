import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RejectdashboardComponent } from './rejectdashboard.component';
import { RejectdashboardRoutingModule } from './rejectdashboard-routing.module';
import { SharedModule } from '../../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from 'src/app/components/common/translate.share.module';
import { NgxGalleryModule } from 'ngx-gallery-9';
 

@NgModule({
  declarations: [RejectdashboardComponent],
  imports: [
    CommonModule,
    RejectdashboardRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
    TranslateSharedModule.forRoot(),
    NgxGalleryModule
  ]
})
export class RejectdashboardModule { 

}
