import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EmailSendRequestModel } from 'src/app/_Models/email-send/email-send-summary.model';
import { map } from 'rxjs/operators';
import { AutoCustomerDecisionRequest, BookingReportRequest, EmailPreviewData, EmailPreviewRequest, EmailRuleRequestByInvoice, InvoiceFileRequest } from 'src/app/_Models/email-send/edit-email-send.model';


@Injectable({ providedIn: 'root' })

export class EditEmailSendService {
  
  url: string

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;
    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }

  getBookingStatus() {
    return this.http.get<any>(`${this.url}/api/EmailSend/`);
  }

  getReportRevision(apiReportId: number,fbReportId: number,requestVersion: number) {
    return this.http.get<any>(`${this.url}/api/EmailSend/getreportVersion/${apiReportId}/${fbReportId}/${requestVersion}`);
  }

  checkFbReportIsInvalidated(fbReportId: number) {
    return this.http.get<any>(`${this.url}/api/EmailSend/checkFbReportIsInvalidated/${fbReportId}`);
  }

  getBookingReportDetails(model: BookingReportRequest) {
    return this.http.post<any>(`${this.url}/api/EmailSend/booking-report-details`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  getInvoiceDetails(model: EmailRuleRequestByInvoice) {
    return this.http.post<any>(`${this.url}/api/EmailSend/invoice-details`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  //delete email send file
  deleteEmailSendFile(id: number) {
    return this.http.delete<any>(`${this.url}/api/EmailSend/${id}`);
  }


  deleteInvoiceSendFile(id: number) {
    return this.http.delete<any>(`${this.url}/api/EmailSend/delete-invoice-file/${id}`);
  }
  //search data
  getFileSendData(model: BookingReportRequest) {
    return this.http.post<any>(`${this.url}/api/EmailSend/email-send-file-details`, model);
  }

    //search data
    getInvoiceFileList(model: InvoiceFileRequest) {
      return this.http.post<any>(`${this.url}/api/EmailSend/invoice-send-file-details`, model);
    }

  getFileTypeList() {
    return this.http.get<any>(`${this.url}/api/EmailSend/get-file-type-data`);
  }
  getInvoiceFileTypeList() {
    return this.http.get<any>(`${this.url}/api/EmailSend/get-invoice-file-types`);
  }
  
  save(model: any) {
    return this.http.post<any>(`${this.url}/api/EmailSend/save`, model);
  }

  saveInvoiceSendAttachments(model: any) {
    return this.http.post<any>(`${this.url}/api/EmailSend/saveInvoiceAttachments`, model);
  }

  getEmailRuleData(model: BookingReportRequest) {
    //bookingIds: Array<number>){
    return this.http.post<any>(`${this.url}/api/EmailSend/get-email-rule-data`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  getEmailRuleDataByInvoiceList(model: EmailRuleRequestByInvoice) {
    return this.http.post<any>(`${this.url}/api/EmailSend/get-email-rule-data-by-invoicelist`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  getEmailDetails(request: EmailPreviewRequest) {
    return this.http.post<any>(`${this.url}/api/EmailSend/email-info-by-email-rule`, request);
  }

  sendEmail(model: Array<EmailPreviewData>) {
    return this.http.post<any>(`${this.url}/api/EmailSend/send-email`, model);
  }

  getEmailSendHistory(inspectionId: number, reportId: number,EmailTypeId:number) 
  {
    return this.http.get<any>(`${this.url}/api/EmailSend/getemailsendhistory/${inspectionId}/${reportId}/${EmailTypeId}`);
  }

  autoCustomerDecisionList(model: AutoCustomerDecisionRequest) {
    return this.http.post<any>(`${this.url}/api/EmailSend/auto-customer-decision`, model);
  }

}
