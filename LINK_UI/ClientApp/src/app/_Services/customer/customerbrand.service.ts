import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, first } from 'rxjs/operators';
import { EditCustomerBrandModel } from '../../_Models/customer/edit-customer-brand.model';
import { CommonCustomerSourceRequest } from 'src/app/_Models/common/common.model';

@Injectable({ providedIn: 'root' })
export class CustomerBrandService {

  url: string
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

  getEditCustomerBrand(id) {
    if (id == null)
      return this.http.get<any>(`${this.url}/api/customerbrand/add`);
    else
      return this.http.get<any>(`${this.url}/api/customerbrand/get/${id}`);
  }

  saveCustomerBrand(model: EditCustomerBrandModel) {

    var id = 10;
    return this.http.post<any>(`${this.url}/api/customerbrand/save`, model)
      .pipe(map(response => {
        return response;
      }));

  }

  getBrandListByCustomerId(model: CommonCustomerSourceRequest, term?: string) {
    model.searchText = "";
    if (term) {
      model.searchText = term;
      model.idList = null;
    }
    return this.http.post<any>(`${this.url}/api/customerbrand/brand-list-by-customer`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }
}
