import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProductCategorySub3Component } from './product-category-sub3.component';


const routes: Routes = [
  {
    path: '',
    component: ProductCategorySub3Component
  },
  {
    path: 'product-category-sub3-summary',
    component: ProductCategorySub3Component
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProductCategorySub3RoutingModule { }
