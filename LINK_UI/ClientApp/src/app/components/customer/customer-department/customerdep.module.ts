import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CustomerdepRoutingModule } from './customerdep-routing.module';
import { CustomerDepartmentComponent } from './customerdepartment.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';
 
@NgModule({
  declarations: [CustomerDepartmentComponent],
  imports: [
    CommonModule,
    CustomerdepRoutingModule,
    SharedModule,
    FormsModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class CustomerdepModule { 

}
