import { Injectable, inject, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { DataSourceResponse } from 'src/app/_Models/common/common.model';
import { map } from 'rxjs/operators';
import { BookingDataSourceRequest, EditExtraFee } from 'src/app/_Models/invoice/editextrafeesinvoice.model';

@Injectable({
  providedIn: 'root'
})
export class EditExtraFeeService {

  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }

  getBookingDataSourceList(model: BookingDataSourceRequest, term?: string) {
    model.searchText = term ? term : model.searchText;
    return this.http.post<any>(`${this.url}/api/ExtraFees/booking-no-details`, model)
      .pipe(map(response => {
        return response.bookings;
      }));
  }

  save(model: EditExtraFee) {
    return this.http.post<any>(`${this.url}/api/ExtraFees`, model);
  }
  getTaxValue(bankId: number, bookingId: number) {
    return this.http.get<any>(`${this.url}/api/ExtraFees/tax/${bankId}/${bookingId}`);
  }
  edit(id: number) {
    return this.http.get<any>(`${this.url}/api/ExtraFees/${id}`);
  }
  getInvoiceNoList(bookingId: number, billedToId: number, serviceId: number) {
    return this.http.get<any>(`${this.url}/api/ExtraFees/invoice/${bookingId}/${billedToId}/${serviceId}`);
  }
  cancel(id: number) {
    return this.http.get<any>(`${this.url}/api/ExtraFees/cancel/${id}`);
  }

  generateManualInvoice(extraFeeId: number) {
    return this.http.get<any>(`${this.url}/api/ExtraFees/generate-manual-invoice/${extraFeeId}`);
  }

  cancelExtraFeeInvoice(id: number) {
    return this.http.put<any>(`${this.url}/api/ExtraFees/cancelExtraFeeInvoice/${id}`, null);
  }
}
