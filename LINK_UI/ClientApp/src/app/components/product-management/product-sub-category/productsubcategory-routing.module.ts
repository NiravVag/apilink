import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProductSubCategoryComponent } from './product-sub-category.component';


const routes: Routes = [
  {
    path: '',
    component: ProductSubCategoryComponent
  },
  {
    path: 'product-subcategory',
    component: ProductSubCategoryComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProductsubcategoryRoutingModule { }
