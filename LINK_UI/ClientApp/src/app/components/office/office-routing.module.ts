import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { OfficeSummaryComponent } from './office-summary/office-summary.component';
import { EditOfficeComponent } from './edit-office/edit-office.component';


const routes: Routes = [
  {
    path: '',
    component: OfficeSummaryComponent
  },
  {
    path: 'office-summary',
    component: OfficeSummaryComponent
  },
  {
    path: 'new-office',
    component: EditOfficeComponent
  },
  {
    path: 'edit-office/:id',
    component: EditOfficeComponent
  },
  {
    path: 'view-office/:id',
    component: EditOfficeComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OfficeRoutingModule { }
