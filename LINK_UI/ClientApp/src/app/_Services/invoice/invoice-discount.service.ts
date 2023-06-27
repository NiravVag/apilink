import { CommonCountySourceRequest, CommonDataSourceRequest } from './../../_Models/common/common.model';
import { InvoiceDiscountModel } from 'src/app/_Models/invoice/invoice-discount-summary.model';
import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { EditInvoiceDiscountModel } from 'src/app/_Models/invoice/invoice-discount-register.model';

@Injectable({
  providedIn: 'root'
})
export class InvoiceDiscountService {

  url: string

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;
    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }

  getInvDisTypes() {
    return this.http.get<any>(`${this.url}/api/invoiceDiscount/get-invoice-discount-type`)
      .pipe(map(response => {
        return response;
      }));
  }

  getInvoiceDiscountSummary(model: InvoiceDiscountModel) {
    return this.http.post<any>(`${this.url}/api/invoiceDiscount/invoice-discount-summary`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  saveInvoiceDiscount(model: EditInvoiceDiscountModel) {
    if (model && model.id > 0) {
      return this.http.post<any>(`${this.url}/api/invoiceDiscount/update-invoice-discount`, model)
        .pipe(map(response => {
          return response;
        }));
    }
    else {
      return this.http.post<any>(`${this.url}/api/invoiceDiscount/save-invoice-discount`, model)
        .pipe(map(response => {
          return response;
        }));
    }

  }
  delete(id: number) {
    return this.http.delete<any>(`${this.url}/api/invoiceDiscount/delete-invoice-discount/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }

  editInvoiceDiscount(id: number) {
    return this.http.get<any>(`${this.url}/api/invoiceDiscount/edit-invoice-discount/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }

  getCustomerBussinessCountries(request: CommonDataSourceRequest) {
    return this.http.post<any>(`${this.url}/api/invoiceDiscount/get-customer-bussiness-countries`,request)
      .pipe(map(response => {
        return response;
      }));
  }
}
