import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserGuideComponent } from './user-guide.component';


const routes: Routes = [
    {
        path: 'user-guide',
        component: UserGuideComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class UsereditRoutingModule { }