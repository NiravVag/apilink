import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, first } from 'rxjs/operators';
import { EmailConfigSummaryModel } from 'src/app/_Models/email-send/email-config-summary.model';
import { EmailSubRequest } from 'src/app/_Models/email-send/email-config.model';

@Injectable({ providedIn: 'root' })
export class EmailConfigurationService {

  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }

  getSpecialRuleList() {
    return this.http.get<any>(`${this.url}/api/EmailConfiguration/get-special-rule-data`);
  }

  getReportInEmailList() {
    return this.http.get<any>(`${this.url}/api/EmailConfiguration/get-report-email-data`);
  }

  getEmailSizeList() {
    return this.http.get<any>(`${this.url}/api/EmailConfiguration/get-email-size-data`);
  }

  getReportSendTypeList(emailTypeId) {
    return this.http.get<any>(`${this.url}/api/EmailConfiguration/get-report-send-type/${emailTypeId}`);
  }

  getEmailSubjectList(request: EmailSubRequest) {
    return this.http.post<any>(`${this.url}/api/EmailConfiguration/get-email-subject-data`, request);
  }

  getStaffNameList() {
    return this.http.get<any>(`${this.url}/api/EmailConfiguration/get-staff-name-data`);
  }

  getCustomerDecisionList(id: number) {
    return this.http.get<any>(`${this.url}/api/EmailConfiguration/get-customer-decision-data/${id}`);
  }
  getEmailSendTypeList() {
    return this.http.get<any>(`${this.url}/api/EmailConfiguration/get-email-send-data`);
  }

  save(model: any) {
    return this.http.post<any>(`${this.url}/api/EmailConfiguration`, model);
  }

  edit(id: number) {
    return this.http.get<any>(`${this.url}/api/EmailConfiguration/${id}`);
  }

  search(model: EmailConfigSummaryModel) {
    return this.http.post<any>(`${this.url}/api/EmailConfiguration/search`, model);
  }

  delete(id: number) {
    return this.http.delete<any>(`${this.url}/api/EmailConfiguration/${id}`);
  }

  getFileNameList(request: EmailSubRequest) {
    return this.http.post<any>(`${this.url}/api/EmailConfiguration/get-email-file-name-data`, request);
  }

  getToAndCCRecipientList(emailTypeId) {
    return this.http.get<any>(`${this.url}/api/EmailConfiguration/get-recipient-type-data/${emailTypeId}`);
  }

  getRecipientTypeList() {
    return this.http.get<any>(`${this.url}/api/EmailConfiguration/get-recipient-data`);
  }

  // getCustomerContactNameList() {
  //   return this.http.get<any>(`${this.url}/api/EmailConfiguration/get-customer-contact-name-data`);
  // }
}