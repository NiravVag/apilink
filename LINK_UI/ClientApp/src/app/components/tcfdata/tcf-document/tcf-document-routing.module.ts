import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { TcfDocumentComponent } from './tcf-document.component';


const routes: Routes = [
  {
    path: '',
    component: TcfDocumentComponent
  },
  {
    path: 'tcf-document',
    component: TcfDocumentComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TCFDocumentRoutingModule { }
