import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { StartingPortRoutingModule } from './starting-port-routing.module';
import { StartingPortComponent } from './starting-port.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';

 

@NgModule({
  declarations: [StartingPortComponent],
  imports: [
    CommonModule,
    StartingPortRoutingModule,
    SharedModule,
    FormsModule,
    NgbModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
   TranslateSharedModule.forRoot()
  ]
})
export class StartingPortModule { 

}
