import { BrowserModule } from '@angular/platform-browser';
import { ErrorHandler, NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS, HttpClient } from '@angular/common/http';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { JwtInterceptor } from './_Services/common/jwt.interceptor';
import { ErrorInterceptor } from './_Services/common/error.interceptor';
import { routing } from './app.routing';
import { NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppTranslateLoader } from './_Services/common/app-translate-loader';
import { ToastrModule } from 'ngx-toastr';
import { AppComponent } from './app.component';
import { MainComponent } from './layouts/main.component';
import { WaitingContent, LinkDateParserFormatter } from './components/common';
import { ErrorComponent } from './components/error/error.component'
import { HeaderComponent } from "./components/shared/header/header.component";
import { WorkerMiddlewareService } from './_Services/common/worker.service';
import { DrawerMenuComponent } from './components/shared/drawer-menu/drawer-menu.component';
import { GlobalErrorHandler } from './components/error/GlobalErrorHandler';
import { NgSelectModule } from '@ng-select/ng-select';
@NgModule({
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    routing,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useClass: AppTranslateLoader,
        deps: [HttpClient]
      },
      isolate:false
    }),
    NgSelectModule,
    BrowserAnimationsModule, // required animations module
    ToastrModule.forRoot() // ToastrModule added
  ],
  declarations: [
    AppComponent,
    MainComponent,
    WaitingContent,
    ErrorComponent,
    HeaderComponent,
    DrawerMenuComponent
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: NgbDateParserFormatter, useClass: LinkDateParserFormatter },
    {provide: ErrorHandler, useClass: GlobalErrorHandler},
     WorkerMiddlewareService
  ],
  entryComponents: [WaitingContent],
  bootstrap: [AppComponent]
})
export class AppModule { }
