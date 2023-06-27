import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DataManagementRoutingModule } from './data-management-routing-module';
import { SharedModule } from '../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateSharedModule } from '../common/translate.share.module';
import { DataManagementSummary } from './data-management-summary/data-management-summary.component';
import { DataManagementEdit } from './data-management-edit/data-management-edit.component';
import { DmUserManagement } from './user-management/dm-user-management.component';
import { TreeviewModule } from 'ngx-treeview';
import { DmUserManagementSummaryComponent } from './dm-user-management-summary/dm-user-management-summary.component';

@NgModule({
  declarations: [DataManagementSummary, DataManagementEdit, DmUserManagement, DmUserManagementSummaryComponent],
  imports: [
    NgbModule,
    CommonModule,
    DataManagementRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgSelectModule,
    NgbPaginationModule,
    TranslateSharedModule.forRoot(),
    TreeviewModule.forRoot()
  ],
  exports: [
    DataManagementEdit
  ]
})


export class DataManagementModule {

}
