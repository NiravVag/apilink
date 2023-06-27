import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, first } from 'rxjs/operators';
import { SubjectConfigModel } from 'src/app/_Models/email-send/subject-config.model';
import { SubConfigSummaryModel } from 'src/app/_Models/email-send/subject-config-summary.model';

@Injectable({ providedIn: 'root' })
export class EmailSubjectService {

  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }

  getFieldColumnList() {
    return this.http.get<any>(`${this.url}/api/EmailSubject/get-field-column-values`);
  }
  save(model: SubjectConfigModel) {
    return this.http.post<any>(`${this.url}/api/EmailSubject`, model);
  }
  edit(id: number) {
    return this.http.get<any>(`${this.url}/api/EmailSubject/${id}`);
  }
  search(model: SubConfigSummaryModel) {
    return this.http.post<any>(`${this.url}/api/EmailSubject/search`, model);
  }
  delete(id: number) {
    return this.http.delete<any>(`${this.url}/api/EmailSubject/${id}`);
  }
  getEmailTypeList() {
    return this.http.get<any>(`${this.url}/api/EmailSubject/get-email-type-data`);
  }
  getModuleList() {
    return this.http.get<any>(`${this.url}/api/EmailSubject/get-module-data`);
  }
  getDateFormats() {
    return this.http.get<any>(`${this.url}/api/EmailSubject/getDateFormats`);
  }
}