import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TemplateSummaryComponent } from './template-summary/templatesummary.component';
import { EditTemplateComponent } from './edit-template/edit-template.component';
import { KpiRoutingModule } from './kpi-routing.module';
import { DndModule } from 'ngx-drag-drop';
import { ViewExportComponent } from './view-export/viewexport.component';
import { SharedModule } from '../shared/shared.module';
import { TranslateSharedModule } from '../common/translate.share.module';
import { InternalBusinessComponent } from './internal-business/internalbusiness.component';
//import { ProgressBarModule } from "angular-progress-bar";
 

@NgModule({
  declarations: [TemplateSummaryComponent, EditTemplateComponent, ViewExportComponent, InternalBusinessComponent],
  imports: [
    CommonModule,
    KpiRoutingModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgbPaginationModule,
    NgSelectModule,
    DndModule,
    //ProgressBarModule,
    TranslateSharedModule.forRoot()
  ]
})
export class kpiModule {

 }
