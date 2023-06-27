import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { BookingNoDataSourceRequest, ComplaintSummaryRequestModel, EditCustomerComplaintModel } from 'src/app/_Models/customer/customer-complaint.model';
import { UserDataSourceRequest } from 'src/app/_Models/common/common.model';

@Injectable({
  providedIn: 'root'
})
export class CustomerComplaintService {
  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;
    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }

  getService() {
    return this.http.get<any>(`${this.url}/api/CSConfig/getService`)
      .pipe(map(response => {
        return response;
      }));
  }
  getComplaintType() {
    return this.http.get<any>(`${this.url}/api/CustomerComplaint/getComplaintType`)
      .pipe(map(response => {
        return response;
      }));
  }
  getComplaintCategory() {
    return this.http.get<any>(`${this.url}/api/CustomerComplaint/getComplaintCategory`)
      .pipe(map(response => {
        return response;
      }));
  }
  // Get the po Data By ID
  getComplaintDetailsById(id: number) {
    return this.http.get<any>(`${this.url}/api/CustomerComplaint/ComplaintDetailsById/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }
  getComplaintRecipientType() {
    return this.http.get<any>(`${this.url}/api/CustomerComplaint/getComplaintRecipientType`)
      .pipe(map(response => {
        return response;
      }));
  }
  getComplaintDepartment() {
    return this.http.get<any>(`${this.url}/api/CustomerComplaint/getComplaintDepartment`)
      .pipe(map(response => {
        return response;
      }));
  }
  getOffice() {
    return this.http.get<any>(`${this.url}/api/audit/GetAuditOffice`).pipe(map(response => {
      return response;
    }));
  }

  getBookingNoDataSourceList(model: BookingNoDataSourceRequest, term?: string) {
    model.searchText = "";
    if (term) {
      model.searchText = term;
    }
    return this.http.post<any>(`${this.url}/api/CustomerComplaint/GetBookingNoDatasource`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }
  getBookingDetailsbyId(bookingId: number) {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/GetBookingInfoDetails/${bookingId}`)
      .pipe(map(response => {
        return response;
      }));
  }
  getBookingProductDetailsbyId(bookingId: number) {
    return this.http.get<any>(`${this.url}/api/CustomerComplaint/GetBookingProductDetails/${bookingId}`)
      .pipe(map(response => {
        return response;
      }));
  }
  getAuditDetailsbyId(auditId: number) {
    return this.http.get<any>(`${this.url}/api/CustomerComplaint/GetAuditDetails/${auditId}`)
      .pipe(map(response => {
        return response;
      }));
  }
  getUserDataSourceList(model: UserDataSourceRequest, term?: string) {
    model.searchText = term ? term : "";
    return this.http.post<any>(`${this.url}/api/UserAccount/getuserdatasource`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }
  saveComplaint(model: EditCustomerComplaintModel) {

    return this.http.post<any>(`${this.url}/api/CustomerComplaint`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  removeComplaintDetailById(id: number) {
    return this.http.get<any>(`${this.url}/api/CustomerComplaint/RemoveComplaintDetail/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }

  getComplaintSummary(model: ComplaintSummaryRequestModel) {
    return this.http.post<any>(`${this.url}/api/CustomerComplaint/GetComplaintSummary`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  delete(id: number) {
    return this.http.delete<any>(`${this.url}/api/CustomerComplaint/${id}`);
  }

  exportSummary(model: ComplaintSummaryRequestModel) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/CustomerComplaint/ExportComplaintSummary`, model, { headers: headers, responseType: 'blob' as 'json' });
  }
}
