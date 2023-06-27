import { DmUserManagementSummaryComponent } from './dm-user-management-summary/dm-user-management-summary.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DataManagementSummary } from './data-management-summary/data-management-summary.component';
import { DataManagementEdit } from './data-management-edit/data-management-edit.component';
import { DmUserManagement } from './user-management/dm-user-management.component';

const routes: Routes = [
    {
        path: 'dmsummary',
        component: DataManagementSummary
    },
    {
      path: 'dmedit/:id',
      component: DataManagementEdit
  },
  {
    path: 'dmadd',
    component: DataManagementEdit
  },
  {
    path: 'dmusermanager',
    component: DmUserManagement
  },
  {
    path: 'dmusermanager/:id',
    component: DmUserManagement
  },
  {
    path: 'dmusermanagersummary',
    component: DmUserManagementSummaryComponent
  }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class DataManagementRoutingModule { }
