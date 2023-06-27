import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { CustomerCollectionModel } from '../../_Models/customer/customercollection.model';
import { CommonCustomerSourceRequest } from 'src/app/_Models/common/common.model';

@Injectable({ providedIn: 'root' })
export class CustomerCollectionService {

  url : string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;
    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }

  getCustomerSummary() {  
    return this.http.get<any>(`${this.url}/api/customer`)
      .pipe(map(response => {
        return response; 
      }));
  }

  getEditCustomerCollection(requestModel: CustomerCollectionModel) {
      return this.http.post<any>(`${this.url}/api/customercollection/get`, requestModel )
      .pipe(map(response => {
        return response;
      }));

  }

  saveCustomerCollection(model: CustomerCollectionModel) {
    return this.http.post<any>(`${this.url}/api/customercollection/save`, model )
      .pipe(map(response => {
        return response;
      }));

  }
  getCollectionListByCustomerId(model: CommonCustomerSourceRequest, term?: string) {
    model.searchText ="";
    if (term) {
      model.searchText = term;
      model.idList = null;
    }
    return this.http.post<any>(`${this.url}/api/customercollection/collection-list-by-customer`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }
}
