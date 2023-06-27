import { Injectable, inject, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, first } from 'rxjs/operators';

import { Observable } from 'rxjs';
import { InspectionBookingsummarymodel } from 'src/app/_Models/booking/inspectionbookingsummarymodel';
import { CustomerDecisionSaveRequest } from 'src/app/_Models/booking/inspectioncustomerdecision';
import { CustomerDecisionListSaveRequest, CustomerDecisionRequestModel } from 'src/app/_Models/booking/customerdecision.model';

@Injectable({
  providedIn: 'root'
})
export class InspectionCustomerDecisionService {

  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }


  GetInspectionCustomerDecisionReportsData(reportId) {
    return this.http.get<any>(`${this.url}/api/InspectionCustomerDecision/GetCustomerDecisionReportsData/${reportId}`);
  }


  GetInspectionCustomerDecision(customerId) {
    if (!customerId) {
      customerId = 0;
    }
    return this.http.get<any>(`${this.url}/api/InspectionCustomerDecision/GetCustomerDecisionList/${customerId}`);
  }

  GetInspectionCustomerDecisionByReport(reportId) {
    return this.http.get<any>(`${this.url}/api/InspectionCustomerDecision/GetCustomerDecision/${reportId}`);
  }

  SaveInspectionCustomerDecision(model: CustomerDecisionSaveRequest) {
    return this.http.post<any>(`${this.url}/api/InspectionCustomerDecision/SaveCustomerDecision`, model)
      .pipe(map(response => {
        return response;
      }));
  }


  CustomerDecisionSummary(model: CustomerDecisionRequestModel) {
    return this.http.post<any>(`${this.url}/api/InspectionCustomerDecision/CustomerDecisionSummary`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  getCustomerDecisionBookingAndProductsList(bookingId: number) {
    return this.http.get<any>(`${this.url}/api/InspectionCustomerDecision/Customer-Decision-Booking-Products/${bookingId}`)
      .pipe(map(response => {
        return response;
      }));
  }

  SaveInspectionCustomerDecisionList(model: CustomerDecisionListSaveRequest) {
    return this.http.post<any>(`${this.url}/api/InspectionCustomerDecision/SaveCustomerDecisionList`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  GetCusDecisionProblematicRemarks(id, reportId) {
    return this.http.get<any>(`${this.url}/api/InspectionCustomerDecision/Customer-Decision-Problematic-Remarks/${id}/${reportId}`);
  }

  exportCustomerDecisionSummary(request: CustomerDecisionRequestModel) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/InspectionCustomerDecision/export-customer-decision`, request, { headers: headers, responseType: 'blob' as 'json' });
  }
}