import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EditEmailSubjectRoutingModule } from './edit-email-subject-routing.module';
import { EditEmailSubjectComponent } from './edit-email-subject.component';
import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateSharedModule } from '../../common/translate.share.module';
import { ValidationPopupComponent } from '../../validation-popup/validation-popup.component';
import { DndModule } from 'ngx-drag-drop';
import { DragDropModule } from '@angular/cdk/drag-drop';

@NgModule({
  declarations: [EditEmailSubjectComponent],
  entryComponents:[ValidationPopupComponent],
  imports: [
    CommonModule,
    EditEmailSubjectRoutingModule,
    SharedModule,
    FormsModule,
    NgSelectModule,
    TranslateSharedModule.forRoot(),
    DndModule,
    DragDropModule
  ]
})
export class EditEmailSubjectModule { 

}
