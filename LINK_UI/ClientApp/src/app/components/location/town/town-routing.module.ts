import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditTownComponent } from './edit-town/edit-town.component'
import {TownSummaryComponent} from './town-summary/town-summary.component'

const routes: Routes = [
  {
    path:'',
    component:TownSummaryComponent
  },
  {
    path:'edit-town/:id',
    component:EditTownComponent
  },
  {
    path:'town-summary/:id',
    component:EditTownComponent
  },
  {
    path:'edit-town',
    component:EditTownComponent
  },
  {
    path: 'town-summary',
    component: TownSummaryComponent
  },    
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TownRoutingModule { }
