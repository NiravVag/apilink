import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProductCategorySub2SummaryComponent } from './product-category-sub2-summary/product-category-sub2-summary.component';
import { EditProductCategorySub2Component } from './edit-product-category-sub2/edit-product-category-sub2.component';


const routes: Routes = [
  {
    path: '',
    component: ProductCategorySub2SummaryComponent
  },
  {
    path: 'product-category-sub2-summary',
    component: ProductCategorySub2SummaryComponent
  },
  {
    path: 'new-product-category-sub2',
    component: EditProductCategorySub2Component
  },
  {
    path: 'edit-product-category-sub2/:id',
    component: EditProductCategorySub2Component
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class Productsub2categoryRoutingModule { }
