import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateSharedModule } from '../../common/translate.share.module';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { ValidationPopupComponent } from '../../validation-popup/validation-popup.component';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { EditInvoiceSendComponent } from './edit-invoice-send.component';
import { EditInvoiceSendRoutingModule } from './edit-invoice-send-routing.module';


@NgModule({
  declarations: [EditInvoiceSendComponent],
  entryComponents:[ValidationPopupComponent],
  imports: [
    CommonModule,
    EditInvoiceSendRoutingModule,
    SharedModule,
    FormsModule,
    NgSelectModule,
    TranslateSharedModule.forRoot(),
    DragDropModule,
    AngularEditorModule
  ]
})
export class EditInvoiceSendModule 
{}
