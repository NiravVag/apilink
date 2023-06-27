import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateSharedModule } from '../../common/translate.share.module';
import { EditExtraFeesComponent } from './edit-extra-fees.component';
import { EditExtraFeesRoutingModule } from './edit-extra-fees-routing.module';
import { ValidationPopupComponent } from '../../validation-popup/validation-popup.component';

@NgModule({
  declarations: [EditExtraFeesComponent],
  entryComponents:[ValidationPopupComponent],
  imports: [
    CommonModule,
    EditExtraFeesRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class EditExtraFeesModule 
{ 
}
