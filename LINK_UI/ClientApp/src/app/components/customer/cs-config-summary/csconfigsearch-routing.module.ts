import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CSConfigSummaryComponent } from './cs-config-summary.component';


const routes: Routes = [
  {
    path: '',
    component:CSConfigSummaryComponent
  },
  {
    path: 'csconfig-summary',
    component:CSConfigSummaryComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CsconfigsearchRoutingModule { }
