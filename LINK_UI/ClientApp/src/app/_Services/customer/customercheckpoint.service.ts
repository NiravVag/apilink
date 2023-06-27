import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { EditCustomerCheckPointModel } from 'src/app/_Models/customer/customer-checkpoint.model';

@Injectable({
  providedIn: 'root'
})
export class CustomerCheckPointService {
  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;
    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }
  getCustomer() {
    return this.http.get<any>(`${this.url}/api/customer`)
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
  getCheckPoint() {
    return this.http.get<any>(`${this.url}/api/CustomerCheckPoint/getCheckPoint`)
      .pipe(map(response => {
        return response;
      }));
  }
  getCustomerCheckPointSummary(cusId: number, serviceId?: number) {
    return this.http.get<any>(`${this.url}/api/CustomerCheckPoint/${cusId}/${serviceId}`)
      .pipe(map(Response => { return Response; }));
  }
  deleteCustomerCheckPoint(id) {
    return this.http.delete<any>(`${this.url}/api/CustomerCheckPoint/${id}`)
      .pipe(map(Response => { return Response; }));
  }
  saveCustomerCheckPoint(model: EditCustomerCheckPointModel) {
    if (!model.id)//add
      return this.http.post<any>(`${this.url}/api/CustomerCheckPoint`, model)
        .pipe(map(response => {
          return response;
        }));
    else
      return this.updateCustomerCheckPoint(model);
  }
  updateCustomerCheckPoint(model: EditCustomerCheckPointModel) {
    return this.http.put<any>(`${this.url}/api/CustomerCheckPoint`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  getCustomerCheckPointDataSource(customerId,serviceId) {
    return this.http.get<any>(`${this.url}/api/CustomerCheckPoint/get-customer-check-point-list-by-service/${customerId}/${serviceId}`)
      .pipe(map(response => { return response; }));
  }

}
