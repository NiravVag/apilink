import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, first } from 'rxjs/operators';
import { EditCustomerModel } from '../../_Models/customer/edit-customer.model';
import { EditCustomerContactModel } from 'src/app/_Models/customer/edit-customercontact.model';
import { EditCustomerServiceConfigModel } from 'src/app/_Models/customer/edit-customerserviceconfig.model';

@Injectable({ providedIn: 'root' })

export class CustomerServiceConfig {
   
   url : string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }


  getCustomerServiceConfig(request) {
    return this.http.post<any>(`${this.url}/api/customerserviceconfig/search`, request)
      .pipe(map(response => {

        return response;
      }));
  }

  getCustomerServiceConfigSummary() {  
    return this.http.get<any>(`${this.url}/api/customerserviceconfig`)
      .pipe(map(response => {
        return response; 
      }));
  }

  getEditCustomerServiceConfig(request) {

    return this.http.post<any>(`${this.url}/api/customerserviceconfig/customerserviceconfig`, request)
      .pipe(map(response => {

        return response;
      }));
  }

  getCustomerServiceConfigMaster(){
      return this.http.get<any>(`${this.url}/api/customerserviceconfig/getserviceconfigmaster`);
  }

  getEditCustomerServiceConfigDetail(id) {

    if (id == null)
      return this.http.get<any>(`${this.url}/api/customerserviceconfig/add`);
    else
      return this.http.get<any>(`${this.url}/api/customerserviceconfig/edit/${id}`);
  }

  saveCustomerServiceConfig(model: EditCustomerServiceConfigModel) {
    return this.http.post<any>(`${this.url}/api/customerserviceconfig/save`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  deleteCustomerServiceConfig(id: number) {
    return this.http.get<any>(`${this.url}/api/customerserviceconfig/delete/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }

  getServiceLevelPickFirst(){
    return this.http.get<any>(`${this.url}/api/customerserviceconfig/getservicelevelpickfirst`);
}

getCustomerServiceTypes(customerId, serviceId) {  
  serviceId = serviceId == null ? 0 : serviceId
  return this.http.get<any>(`${this.url}/api/customerserviceconfig/getserviceType/${customerId}/${serviceId}`)
    .pipe(map(response => {
      return response; 
    }));
}
}