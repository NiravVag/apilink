import { Injectable, inject, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { InspectionPicking } from 'src/app/_Models/booking/inspectionpicking.model';
import { map, first } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { CustomkpiModel } from 'src/app/_Models/kpi/customkpimodel';
@Injectable({
  providedIn: 'root'
})
export class InspectionPickingService {
  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }

  GetLabList(customerId) {
    return this.http.get<any>(`${this.url}/api/InspectionPicking/GetLabList/${customerId}`);
  }

  GetInspectionPickingList(bookingId) {
    return this.http.get<any>(`${this.url}/api/InspectionPicking/${bookingId}`);
  }

  GetPickingProducts(bookingId) {
    return this.http.get<any>(`${this.url}/api/InspectionPicking/GetPickingProducts/${bookingId}`);
  }

  GetLabAddressList(labID) {
    return this.http.get<any>(`${this.url}/api/InspectionPicking/GetLabAddressListByLab/${labID}`);
  }
  GetLabContactList(labID, customerID) {
    return this.http.get<any>(`${this.url}/api/InspectionPicking/GetLabContactList/${labID}/${customerID}`);
  }
  GetCustomerContacts(customerID) {
    return this.http.get<any>(`${this.url}/api/InspectionPicking/GetCustomerContacts/${customerID}`);
  }
  GetPickingPdf(bookingId) {
    return this.http.get(`${this.url}/api/InternalFBReports/GetQcPicking/${bookingId}`, { responseType: 'blob' });
  }

  saveInspectionPicking(model: Array<InspectionPicking>, bookingId) {
    return this.http.post<any>(`${this.url}/api/InspectionPicking/SaveInspectionPicking/${bookingId}`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  exportSummary(request: CustomkpiModel) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/InspectionPicking/ExportInspectionPicking`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

  

}