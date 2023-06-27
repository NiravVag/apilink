import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CustomerCollectionRoutingModule } from './customer-collection-routing.module';
import { CustomerCollectionComponent } from './customer-collection.component';

import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
 
import { TranslateSharedModule } from '../../common/translate.share.module';
import { NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
 
@NgModule({
  declarations: [CustomerCollectionComponent],
  imports: [
    CommonModule,
    CustomerCollectionRoutingModule,
    SharedModule,
    FormsModule,
    NgSelectModule,
   TranslateSharedModule.forRoot(),
   NgbPaginationModule
  ]
})
export class CustomerCollectionModule {

 }
