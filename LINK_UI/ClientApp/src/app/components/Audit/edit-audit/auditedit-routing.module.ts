import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditAuditComponent } from './edit-audit.component';


const routes: Routes = [
  {
    path:'',
    component:EditAuditComponent
  },
  {
    path:'edit-audit',
    component:EditAuditComponent
  },
  {
    path: 'edit-audit/:id/:type',
    component: EditAuditComponent
  },
  {
    path:'edit-audit/:id',
    component:EditAuditComponent
  },
  {
    path:'view-audit/:id',
    component:EditAuditComponent
  }
 
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuditeditRoutingModule { }
