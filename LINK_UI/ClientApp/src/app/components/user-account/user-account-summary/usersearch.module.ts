import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UsersearchRoutingModule } from './usersearch-routing.module';
import { UserAccountSummaryComponent } from './user-account-summary.component';


import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgbDatepickerModule, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';
import { NgSelectModule } from '@ng-select/ng-select';
import { HttpClient } from '@angular/common/http';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { TranslateModule, TranslateLoader, TranslateService } from '@ngx-translate/core';
import { TranslateSharedModule } from '../../common/translate.share.module';

// AoT requires an exported function for factories
//export function HttpLoaderFactory(httpClient: HttpClient) {
//  var loader = new TranslateHttpLoader(httpClient, './assets/i18n/');
//  return loader;
//}

@NgModule({
  declarations: [UserAccountSummaryComponent],
  imports: [
    CommonModule,
    UsersearchRoutingModule,
    NgbPaginationModule,
    SharedModule,
    FormsModule,
    NgbDatepickerModule,
    NgSelectModule,
    TranslateSharedModule.forRoot()
  ]
})
export class UsersearchModule {

}
