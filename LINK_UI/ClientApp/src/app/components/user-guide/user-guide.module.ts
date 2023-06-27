import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UsereditRoutingModule } from './user-guide.routing.module';
import { UserGuideComponent } from './user-guide.component';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { DndModule } from 'ngx-drag-drop';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { TranslateSharedModule } from '../common/translate.share.module';

@NgModule({
  declarations: [UserGuideComponent],
  imports: [
    CommonModule,
    UsereditRoutingModule,
    FormsModule,
    NgSelectModule,
    TranslateSharedModule.forRoot(),
    DndModule,
    DragDropModule
  ]
})
export class UserGuideModule { 

}