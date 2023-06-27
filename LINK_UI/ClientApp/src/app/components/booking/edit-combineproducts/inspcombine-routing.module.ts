import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditCombineOrdersComponent } from './edit-combineorders.component';


const routes: Routes = [
  {
    path: 'edit-combineorders/:id',
    component: EditCombineOrdersComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class InspcombineRoutingModule { }
