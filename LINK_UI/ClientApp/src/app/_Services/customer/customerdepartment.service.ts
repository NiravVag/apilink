import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, first } from 'rxjs/operators';
import { EditCustomerModel } from '../../_Models/customer/edit-customer.model';
import { CustomerDepartmentmodel } from 'src/app/_Models/customer/customerdepartment.model';
import { ToastrService } from "ngx-toastr";
import { CommonCustomerSourceRequest } from 'src/app/_Models/common/common.model';
@Injectable({ providedIn: 'root' })

export class CustomerDepartmentService {

  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }

  getCustomerDepartment(id) {
    return this.http.get<any>(`${this.url}/api/customerdepartment/get/${id}`);
    //return this.http.get<any>(`${this.url}/api/customercontact/edit`);
  }

  saveDepartment(model: CustomerDepartmentmodel) {
    return this.http.post<any>(`${this.url}/api/customerdepartment/department/save`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  deleteCustomerDepartment(id: number) {
    return this.http.get<any>(`${this.url}/api/customerdepartment/delete/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }
  getDeptListByCustomerId(model: CommonCustomerSourceRequest, term?: string) {
    model.searchText = "";
    if (term) {
      model.searchText = term;
      model.idList = null;
    }
    return this.http.post<any>(`${this.url}/api/customerdepartment/dept-list-by-customer`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }
}
