import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CountrySummaryComponent } from './country-summary/country-summary.component';
import { EditCountryComponent } from './edit-country/edit-country.component';


const routes: Routes = [
  {
    path: '',
    component: CountrySummaryComponent
  },
  {
    path: 'country-summary',
    component: CountrySummaryComponent
  },
  {
    path: 'edit-country',
    component: EditCountryComponent
  },
  {
    path: 'edit-country/:id',
    component: EditCountryComponent
  },
  {
    path: 'view-country/:id',
    component: EditCountryComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CountryRoutingModule { }
