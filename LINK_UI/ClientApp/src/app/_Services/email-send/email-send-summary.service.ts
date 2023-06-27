import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EmailSendRequestModel } from 'src/app/_Models/email-send/email-send-summary.model';
import { map } from 'rxjs/operators';


@Injectable({ providedIn: 'root' })

export class EmailSendSummaryService {

  url: string

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;
    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }

  getBookingStatus() 
  {
    return this.http.get<any>(`${this.url}/api/EmailSend/booking-status-data`);
  }

  getAeList() 
  {
    return this.http.get<any>(`${this.url}/api/EmailSend/ae-list`);
  }

  searchEmailSummary(model: EmailSendRequestModel){
    return this.http.post<any>(`${this.url}/api/EmailSend/email-send-summary-search`, model)
    .pipe(map(response => 
      {
        return response;
      }));
  }

  validateMutipleEmailSendByCustomer(customerId: number, serviceId: number) 
  {
    return this.http.get<any>(`${this.url}/api/EmailSend/email-multiple-send-validation/${customerId}/${serviceId}`);
  } 
  
  emailSendSummaryExport(request: EmailSendRequestModel) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/EmailSend/ExportEmailSendSummary`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

}
