import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditTravelTariffComponent } from './edit-travel-tariff.component';


const routes: Routes = [
  {
    path: '',
    component: EditTravelTariffComponent
  },
  {
    path: 'edit-travel-tariff',
    component: EditTravelTariffComponent
  },
  {
    path: 'edit-travel-tariff/:id',
    component: EditTravelTariffComponent
  }  
];
 
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EditTravelTariffRoutingModule { }
