import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { map } from "rxjs/operators";
import { CurrencyRateModel } from "../../_Models/currency/currencyrate.model";
import { CurrencyRateItem } from "../../_Models/currency/echange-rate.model";
import { RateMatrixModel } from "../../_Models/currency/ratematrix.model";


@Injectable({ providedIn: 'root' })
export class CurrencyService {


  url: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }


  getExchangeRateSummary() {
    return this.http.get<any>(`${this.url}/api/ExchangeRate`)
      .pipe(map(response => {
        return response;
      }));
  }

  getDataRate(model: CurrencyRateModel) {   
    return this.http.post<any>(`${this.url}/api/ExchangeRate/Search`, model).pipe(map(response => {
        return response;
      }));
  }

  save(request:any) {
    return this.http.post<any>(`${this.url}/api/ExchangeRate/save`, request).pipe(map(response => {
      return response;
    }));
  }

  getDataMatrix(request: RateMatrixModel) {
    return this.http.post<any>(`${this.url}/api/ExchangeRate/GetMatrix`, request).pipe(map(response => {
      return response;
    }));
  }

  getFile(currencyid:number, fromDate:string, toDate:string, typeid:number) {
    return this.http.get(`${this.url}/api/ExchangeRate/export/${currencyid}/${fromDate}/${toDate}/${typeid}`, { responseType: 'blob' });
  }



}
