import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditCountyComponent } from './edit-county/edit-county.component'
import {CountySummaryComponent} from './county-summary/county-summary.component'

const routes: Routes = [
  {
    path:'',
    component:CountySummaryComponent
  },
  {
    path:'edit-county/:id',
    component:EditCountyComponent
  },
  {
    path:'county-summary/:id',
    component:EditCountyComponent
  },
  {
    path:'edit-county',
    component:EditCountyComponent
  },
  {
    path: 'county-summary',
    component: CountySummaryComponent
  },    
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CountyRoutingModule { }
