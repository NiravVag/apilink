import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EditEmailSendRoutingModule } from './edit-email-send-routing.module';
import { EditEmailSendComponent } from './edit-email-send.component';
import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateSharedModule } from '../../common/translate.share.module';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { ValidationPopupComponent } from '../../validation-popup/validation-popup.component';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { PipesModule } from '../../shared/pipe/pipes.module';


@NgModule({
  declarations: [EditEmailSendComponent],
  entryComponents:[ValidationPopupComponent],
  imports: [
    CommonModule,
    EditEmailSendRoutingModule,
    SharedModule,
    FormsModule,
    NgSelectModule,
    TranslateSharedModule.forRoot(),
    DragDropModule,
    AngularEditorModule,
    PipesModule
  ]
})
export class EditEmailSendModule { 

}
