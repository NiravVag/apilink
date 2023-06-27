import { Injectable, inject, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, first } from 'rxjs/operators';
import { EditAuditmodel, AttachmentFile } from "../../_Models/Audit/edit-auditmodel";
import { Auditsummarymodel } from "../../_Models/Audit/auditsummarymodel";
import { Auditcancelmodel } from 'src/app/_Models/Audit/auditcancelmodel';
import { Observable } from 'rxjs';
import { Auditreportmodel } from 'src/app/_Models/Audit/auditreportmodel';
import { auditcusreportrequest } from 'src/app/_Models/Audit/auditcusreportmodel';
@Injectable({
  providedIn: 'root'
})
export class AuditService {

  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }

  GetCustomerByUserType() {
    return this.http.get<any>(`${this.url}/api/Customer/GetCustomerByUsertType`);
  }
  GetSeasonYear() {
    return this.http.get<any>(`${this.url}/api/reference/GetSeasonYear`);
  }
  GetEvaluationRound() {
    return this.http.get<any>(`${this.url}/api/audit/GetAuditEvaluationRound`);
  }
  GetAuditor() {
    return this.http.get<any>(`${this.url}/api/HumanResource/GetAuditor`);
  }
  GetAuditType() {
    return this.http.get<any>(`${this.url}/api/audit/GetAuditType`);
  }
  GetAuditWorkProcess() {
    return this.http.get<any>(`${this.url}/api/audit/GetAuditWorkprocess`);
  }
  GetAuditBookingContact(factid) {
    return this.http.get<any>(`${this.url}/api/audit/GetAuditBookingContactDetails/${factid}`);
  }
  GetAuditBookingRules(cusid, factid) {
    return this.http.get<any>(`${this.url}/api/audit/GetAuditBookingRuleDetails/${cusid},${factid}`);
  }
  GetAuditCSDetails(factid, cusid) {
    return this.http.get<any>(`${this.url}/api/audit/GetAuditCS/${factid},${cusid}`);
  }
  GetOffice() {
    return this.http.get<any>(`${this.url}/api/audit/GetAuditOffice`);
  }
  EditAudit(id) {
    if (id == null)
      return this.http.get<any>(`${this.url}/api/audit/add`);
    else
      return this.http.get<any>(`${this.url}/api/audit/EditAudit/${id}`);
  }
  GetAuditDetailsByCusId(id) {
    return this.http.get<any>(`${this.url}/api/audit/GetAuditDetailsByCustomerId/${id}`);
  }
  GetSupplierDetailsByCusIdSupId(supid, cusid) {
    if (cusid != null)
      return this.http.get<any>(`${this.url}/api/audit/GetSupplierDetailsById/${cusid},${supid}`);
    else
      return this.http.get<any>(`${this.url}/api/audit/GetSupplierDetailsById/${supid}`);
  }
  GetFactoryDetailsByCusIdFactId(factid, cusid) {
    if (cusid != null)
      return this.http.get<any>(`${this.url}/api/audit/GetFactoryDetailsById/${cusid},${factid}`);
    else
      return this.http.get<any>(`${this.url}/api/audit/GetFactoryDetailsById/${factid}`);
  }
  saveAudit(model: EditAuditmodel) {
    return this.http.post<any>(`${this.url}/api/audit/save`, model)
      .pipe(map(response => {
        return response;
      }));
  }
  uploadAttachedFiles(auditid, attachedList: Array<AttachmentFile>) {

    const formData = new FormData();

    attachedList.map(x => {
      formData.append(x.uniqueld, x.file);
    });

    return this.http.post(`${this.url}/api/audit/attached/${auditid}`, formData)
      .pipe(map(response => {
        return response;
      }));

  }
  getFile(id) {
    return this.http.get(`${this.url}/api/audit/file/${id}`, { responseType: 'blob' });
  }
  Getauditsummary() {
    return this.http.get<any>(`${this.url}/api/audit/Getauditsummary`);
  }
  Getsupplierbycusid(cusid, isBookingRequest = false) {
    let apiUrl = `${this.url}/api/Supplier/GetsupplierBycustomerid/${cusid}`;
    if (isBookingRequest)
      apiUrl = apiUrl + "?isBookingRequest=" + isBookingRequest;
    return this.http.get<any>(apiUrl).pipe(map(response => { return response; }));
  }
  Getfactorybysupplierid(supid) {
    return this.http.get<any>(`${this.url}/api/Supplier/GetfactoryBysupid/${supid}`);
  }
  SearchAuditSummary(model: Auditsummarymodel) {
    return this.http.post<any>(`${this.url}/api/audit/SearchAuditSummary`, model)
      .pipe(map(response => {
        return response;
      }));
  }
  exportSummary(request: Auditsummarymodel) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/audit/ExportAuditSearchSummary`, request, { headers: headers, responseType: 'blob' as 'json' });
  }
  GetCancelauditDetails(id, typeid) {
    return this.http.get<any>(`${this.url}/api/audit/GetCancelEditAuditDetails/${id},${typeid}`);
  }
  SaveCancelAudit(model: Auditcancelmodel) {
    return this.http.post<any>(`${this.url}/api/audit/SaveCancelrescheduleAudit`, model)
      .pipe(map(response => {
        return response;
      }));
  }
  GetAuditDetailsForReport(id) {
    return this.http.get<any>(`${this.url}/api/audit/GetAuditBasicDetails/${id}`);
  }
  getAuditReportFiles(id) {
    return this.http.get(`${this.url}/api/audit/reportfiles/${id}`, { responseType: 'blob' });
  }
  saveAuditReport(model: Auditreportmodel) {
    return this.http.post<any>(`${this.url}/api/audit/saveauditreport`, model)
      .pipe(map(response => {
        return response;
      }));
  }
  GetScheduledAuditor(id) {
    return this.http.get<any>(`${this.url}/api/audit/GetScheduledAuditor/${id}`);
  }
  GetAuditReportDetails(id) {
    return this.http.get<any>(`${this.url}/api/audit/GetAuditReportDetails/${id}`);
  }
  uploadReportFiles(auditid, attachedList: Array<AttachmentFile>) {

    const formData = new FormData();

    attachedList.map(x => {
      formData.append(x.uniqueld, x.file);
    });

    return this.http.post(`${this.url}/api/audit/reportfiles/${auditid}`, formData)
      .pipe(map(response => {
        return response;
      }));

  }
  //get audit status
  GetAuditStatus() {
    return this.http.get<any>(`${this.url}/api/audit/GetAuditStatus`)
      .pipe(map(response => {
        return response;
      }));
  }

  //get audit service type by customer id

  GetAuditServiceType(customerid) {
    return this.http.get<any>(`${this.url}/api/audit/GetAuditServiceType/${customerid}`)
      .pipe(map(response => {
        return response;
      }));
  }

  // get audit cus report

  SearchAuditcusReport(model: auditcusreportrequest) {
    return this.http.post<any>(`${this.url}/api/AuditCusReport/auditcussearch`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  getProductCategory(customerId, serviceTypeId){
    return this.http.get<any>(`${this.url}/api/audit/GetProductCategory/${customerId},${serviceTypeId}`);
  }
}
