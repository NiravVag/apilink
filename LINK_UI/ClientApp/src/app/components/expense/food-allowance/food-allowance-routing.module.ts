import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { FoodAllowanceComponent } from './food-allowance.component';


const routes: Routes = [
  {
    path: '',
    component: FoodAllowanceComponent
  },
  {
    path: 'food-allowance',
    component: FoodAllowanceComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class FoodAllowanceRoutingModule { }
