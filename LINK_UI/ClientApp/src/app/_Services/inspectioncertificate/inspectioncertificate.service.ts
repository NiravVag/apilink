import { Injectable, inject, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, first } from 'rxjs/operators';

import { InspectionBookingsummarymodel, HolidayRequest } from "../../_Models/booking/inspectionbookingsummarymodel";
import { Observable } from 'rxjs';
import { EditInspectionBooking, InspectionFileAttachment, ProductFileAttachment, BookingCustomerContactRequest } from '../../_Models/booking/inspectionbooking.model';
import { BookingSearchRedirectPage } from '../../components/common/static-data-common';
import { SplitBooking } from 'src/app/_Models/booking/splitbookingmodel';
import { InspectionCertificateBookingSearchRequest, InspectionCertificateRequest, ICBookingProductRequest } from 'src/app/_Models/inspectioncertificate/inspectioncertificate.model';
import { ICSummaryModel } from 'src/app/_Models/inspectioncertificate/inspectioncertificatesummary.model';

@Injectable({
  providedIn: 'root'
})
export class InspectionCertificateService {

  url: string;
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }

  BookingSearch(model: InspectionCertificateBookingSearchRequest) {
    return this.http.post<any>(`${this.url}/api/InspectionCertificate/bookingSearch`, model)
      .pipe(map(response => {
        return response;
      }));
  }
  ProductBookingIC(model: ICBookingProductRequest) {
    return this.http.post<any>(`${this.url}/api/InspectionCertificate/bookingICProduct`, model)
      .pipe(map(response => {
        return response;
      }));
  }
  SupplierHeadOfficeAddress(supId) {
    return this.http.get<any>(`${this.url}/api/Supplier/GetSupplierHeadOfficeAddress/${supId}`);
  }
  SaveIC(model: InspectionCertificateRequest) {
    return this.http.post<any>(`${this.url}/api/InspectionCertificate/save`, model)
      .pipe(map(response => {
        return response;
      }));
  }
  EditIC(id: number) {
    return this.http.get<any>(`${this.url}/api/InspectionCertificate/edit/${id}`);
  }
  CancelIC(id: number) {
    return this.http.get<any>(`${this.url}/api/InspectionCertificate/cancel/${id}`);
  }
  preview(id: number, isDraft: boolean) {
    return this.http.get(`${this.url}/api/InspectionCertificate/preview/${id}/${isDraft}`, { responseType: 'blob' });
  }
  GetICStatusList() {
    return this.http.get<any>(`${this.url}/api/InspectionCertificate/icstatuslist`);
  }
  SearchICSummary(model: ICSummaryModel) {
    return this.http.post<any>(`${this.url}/api/InspectionCertificate/icsummarysearch`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  SearchICProducts(id: number) {

    return this.http.get<any>(`${this.url}/api/InspectionCertificate/icproducts/${id}`);
  }
  GetICTitleList() {
    return this.http.get<any>(`${this.url}/api/InspectionCertificate/icTitleList`);
  }

  GetCustomerByCheckPointUserType() {
    return this.http.get<any>(`${this.url}/api/Customer/getCustomerByCheckPointUsertType`);
  }  
}
