import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CSConfigRegisterComponent } from './cs-config-register.component';


const routes: Routes = [
  {
    path: '',
    component:CSConfigRegisterComponent
  },
  {
    path: 'csconfig-register',
    component:CSConfigRegisterComponent
  },
  {
    path: 'csconfig-register/:id',
    component:CSConfigRegisterComponent
  }
 
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CsconfigregRoutingModule { }
