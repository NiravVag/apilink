import { Injectable, inject, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, first } from 'rxjs/operators';

import { CustomerReportModel, StatusRequest } from "../../_Models/Report/reportmodel";
import { Observable } from 'rxjs';
import { EditInspectionBooking, InspectionFileAttachment, ProductFileAttachment, BookingCustomerContactRequest } from '../../_Models/booking/inspectionbooking.model';
import { BookingSearchRedirectPage } from '../../components/common/static-data-common';
import { SplitBooking } from 'src/app/_Models/booking/splitbookingmodel';
import { InspectionOccupancyModel } from 'src/app/_Models/report/inspection-occupancy.model';

@Injectable({
  providedIn: 'root'
})
export class ReportService {

  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }


  SearchReportSummary(model: CustomerReportModel) {
    return this.http.post<any>(`${this.url}/api/Report/Search`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  SearchBookingProducts(bookingId: number) {
    return this.http.get<any>(`${this.url}/api/Report/GetProducts/${bookingId}`)
      .pipe(map(response => {
        return response;
      }));
  }

  SearchBookingContainers(bookingId: number) {
    return this.http.get<any>(`${this.url}/api/Report/GetContainers/${bookingId}`)
      .pipe(map(response => {
        return response;
      }));
  }

  exportSummary(request: CustomerReportModel) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/Report/ExportReportSummary`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

  Getreportsummary() {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/Getbookingsummary`);
  }

  Updatestatus(model: StatusRequest) {
    return this.http.post<any>(`${this.url}/api/Report/StatusUpdate`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  getInspectionOccupancySummary(model: InspectionOccupancyModel) {
    return this.http.post<any>(`${this.url}/api/Report/getInspectionOccupancy`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  exportInspectionOccupancy(model: InspectionOccupancyModel) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/Report/exportInspectionOccupancy`, model, { headers: headers, responseType: 'blob' as 'json' });
  }
}
