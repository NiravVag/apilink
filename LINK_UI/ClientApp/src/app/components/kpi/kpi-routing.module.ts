import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TemplateSummaryComponent } from './template-summary/templatesummary.component';
import { EditTemplateComponent } from './edit-template/edit-template.component';
import { ViewExportComponent } from './view-export/viewexport.component';
import { InternalBusinessComponent } from './internal-business/internalbusiness.component';

const routes: Routes = [
  {
    path:'templates-summary',
    component: TemplateSummaryComponent
  },
  {
    path:'register-template',
    component: EditTemplateComponent
  },
  {
    path: 'edit-template/:id',
    component: EditTemplateComponent
  },
  {
    path: 'view-export/:id',
    component: ViewExportComponent
  },
  {
    path: 'internal-business',
    component: InternalBusinessComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class KpiRoutingModule {
  
 }
