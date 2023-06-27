import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CitySummaryComponent } from './city-summary/city-summary.component';
import { EditCityComponent } from './edit-city/edit-city.component';


const routes: Routes = [
  {
    path: '',
    component: CitySummaryComponent
  },
  {
    path: 'city-summary',
    component: CitySummaryComponent
  },
  {
    path: 'edit-city',
    component: EditCityComponent
  },
  {
    path: 'edit-city/:id',
    component: EditCityComponent
  },
  {
    path: 'view-city/:id',
    component: EditCityComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CityRoutingModule { }
