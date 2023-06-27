import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProvinceSummaryComponent } from './province-summary/province-summary.component';
import { EditProvinceComponent } from './edit-province/edit-province.component';


const routes: Routes = [
  {
    path:'',
    component:ProvinceSummaryComponent
  },
  {
    path:'province-summary',
    component:ProvinceSummaryComponent
  },
  {
    path:'edit-province/:id',
    component:EditProvinceComponent
  },
  {
    path:'view-province/:id',
    component:EditProvinceComponent
  },
  {
    path:'edit-province',
    component:EditProvinceComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProvinceRoutingModule { }
