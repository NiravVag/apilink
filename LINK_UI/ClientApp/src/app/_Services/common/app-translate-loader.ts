import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslateLoader } from '@ngx-translate/core';
import { Observable } from 'rxjs';
const config = require("../../../assets/appconfig/appconfig.json");

@Injectable({ providedIn: 'root' })

export class AppTranslateLoader implements TranslateLoader  {
  constructor(private httpClient: HttpClient) {}

  getTranslation(lang: string): Observable<any> {
     return this.httpClient.get<any>(config.APP.APISERVER+`api/User/TranslateFile/${lang}`)
  }
}