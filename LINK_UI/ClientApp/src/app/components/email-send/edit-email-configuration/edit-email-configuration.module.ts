import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateSharedModule } from '../../common/translate.share.module';
import { ValidationPopupComponent } from '../../validation-popup/validation-popup.component';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { EditEmailConfigurationComponent } from './edit-email-configuration.component';
import { EditEmailConfigurationRoutingModule } from './edit-email-configuration-routing.module';

@NgModule({
  declarations: [EditEmailConfigurationComponent],
  entryComponents:[ValidationPopupComponent],
  imports: [
    CommonModule,
    EditEmailConfigurationRoutingModule,
    SharedModule,
    FormsModule,
    NgSelectModule,
    TranslateSharedModule.forRoot(),
    NgbPaginationModule
  ]
})
export class EditEmailConfigurationModule { 

}
