import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { OtherMandayComponent } from './other-manday.component';


const routes: Routes = [
  {
    path: '',
    component: OtherMandayComponent
  },
  {
    path: 'other-manday',
    component: OtherMandayComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OtherMandayRoutingModule { }
