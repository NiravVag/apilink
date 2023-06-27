import { CsAllocationSearchModel } from './../../_Models/customer/cs-allocation-summary.model';
import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { editCSConfigModel } from 'src/app/_Models/customer/csconfig-register.model';
import { csDeleteItem, csConfigSearchModel } from 'src/app/_Models/customer/csconfig-summary.model';
import { EditCSAllocationModel } from 'src/app/_Models/customer/cs-allocation.model';
@Injectable({ providedIn: 'root' })
export class CSConfigService {
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
  getOffices(id: any) {
    return this.http.get<any>(`${this.url}/api/HumanResource/OfficesControl/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }
  getProductCategoryList() {
    return this.http.get<any>(`${this.url}/api/customerproduct/getProductsCategory`)
      .pipe(map(response => {
        return response;
      }));
  }
  getCustomerService() {
    return this.http.get<any>(`${this.url}/api/CSConfig/getCustomerService`)
      .pipe(map(response => {
        return response;
      }));
  }
  getService() {
    return this.http.get<any>(`${this.url}/api/CSConfig/getService`)
      .pipe(map(response => {
        return response;
      }));
  }
  saveCSConfigRegister(model: editCSConfigModel) {
    return this.http.post<any>(`${this.url}/api/CSConfig/saveCSConfigRegister`, model)
      .pipe(map(response => {
        return response;
      }));
  }
  editCSConfigRegister(id: number) {
    return this.http.get<any>(`${this.url}/api/CSConfig/editCSConfig/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }
  SearchCustomerServiceConfigSummary(model: csConfigSearchModel) {
    return this.http.post<any>(`${this.url}/api/CSConfig/searchCSConfig`, model)
      .pipe(map(response => {
        return response;
      }));
  }
  deleteCSConfig(removeModel: csDeleteItem) {
    return this.http.post<any>(`${this.url}/api/CSConfig/deleteCSConfig`, removeModel)
      .pipe(map(response => {
        return response;
      }));
  }


  getCustomerAllocation(model: CsAllocationSearchModel) {
    return this.http.post<any>(`${this.url}/api/CSConfig/getCustomerAllocation`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  saveCustomerAllocation(model: EditCSAllocationModel) {
    return this.http.post<any>(`${this.url}/api/CSConfig/saveCustomerAllocation`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  deleteCSAllocations(model: csDeleteItem) {
    return this.http.post<any>(`${this.url}/api/CSConfig/deleteCustomerAllocations`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  getCSAllocation(id: number) {
    return this.http.get<any>(`${this.url}/api/CSConfig/get-cs-allocation/${id}`)
    .pipe(map(response => {
      return response;
    }));
  }

  exportSummary(request) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/CSConfig/Export-cs-allocation`, request, { headers: headers, responseType: 'blob' as 'json' });
  }
}
