import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, first } from 'rxjs/operators';
import { EditCustomerModel } from '../../_Models/customer/edit-customer.model';
import { EditCustomerContactModel } from 'src/app/_Models/customer/edit-customercontact.model';
import { CustomerContactUserRequest } from 'src/app/_Models/customer/customer-contact-user-request';

@Injectable({ providedIn: 'root' })

export class CustomerContactService {
   
   url : string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }

  getEditCustomerContact(request) {
    return this.http.post<any>(`${this.url}/api/customer/customercontact/search`, request)
      .pipe(map(response => {

        return response;
      }));
  }

  getEditCustomerContactSummary(request) {

    return this.http.post<any>(`${this.url}/api/customercontact/customercontactsummary`, request)
      .pipe(map(response => {

        return response;
      }));
  }

  getAddCustomerContactSummary(id) {

    return this.http.get<any>(`${this.url}/api/customercontact/add/${id}`);
  }

  getCustomerContactSummary(request) {
    return this.http.post<any>(`${this.url}/api/customercontact/search`, request)
      .pipe(map(response => {

        return response;
      }));
  }

  deleteCustomerContact(id: number) {
    return this.http.get<any>(`${this.url}/api/customercontact/deletecontact/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }

  getEditCustomer(id) {

    if (id == null)
      return this.http.get<any>(`${this.url}/api/customercontact/add`);
    else
      return this.http.get<any>(`${this.url}/api/customercontact/edit/${id}`);
  }

  saveCustomerContact(model: EditCustomerContactModel) {
    return this.http.post<any>(`${this.url}/api/customercontact/save`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  saveContactUserCredential(customerContactUserRequest: CustomerContactUserRequest) {
    return this.http.post<any>(`${this.url}/api/CustomerContact/create-customer-contact-credential`, customerContactUserRequest)
      .pipe(map(response => {
        return response;
      }));
  }
  getContactBrandByCusId(cusId) {
    return this.http.get<any>(`${this.url}/api/customercontact/GetContactBrandByCusId/${cusId}`).pipe(map(response => { return response; }));
  }

  getCustomerContactByBooking(bookingId) {
    return this.http.get<any>(`${this.url}/api/customercontact/GetCustomerContactByBooking/${bookingId}`).pipe(map(response => { return response; }));
  }

  getCustomerContactByBookingAndService(bookingId,serviceId) {
    return this.http.get<any>(`${this.url}/api/customercontact/GetCustomerContactByServiceAndBooking/${bookingId}/${serviceId}`).pipe(map(response => { return response; }));
  }

  getCustomerContactByCustomerId(customerId) {
    return this.http.get<any>(`${this.url}/api/customercontact/get-customer-contact-by-customerId/${customerId}`).pipe(map(response => { return response; }));
  }
}