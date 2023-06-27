import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, first } from 'rxjs/operators';
import { CustomerBuyermodel } from 'src/app/_Models/customer/customerbuyer.model';
import { CommonCustomerSourceRequest } from 'src/app/_Models/common/common.model';


@Injectable({
  providedIn: 'root'
})
export class CustomerbuyerService {

  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;
    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }

  getCustomerBuyer(id) {
    return this.http.get<any>(`${this.url}/api/customerbuyer/${id}`);
  }

  saveBuyer(model: CustomerBuyermodel) {
    return this.http.post<any>(`${this.url}/api/customerbuyer`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  deleteCustomerBuyer(id: number) {
    return this.http.delete<any>(`${this.url}/api/CustomerBuyer/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }
  getBuyerListByCustomerId(model: CommonCustomerSourceRequest, term?: string) {
    model.searchText = "";
    if (term) {
      model.searchText = term;
      model.idList = null;
    }
    return this.http.post<any>(`${this.url}/api/customerbuyer/buyer-list-by-customer`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }
}

