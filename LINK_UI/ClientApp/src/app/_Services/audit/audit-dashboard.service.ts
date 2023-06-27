import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { AuditDashboardMapFilterRequest, AuditDashboardModel, AuditDashboardResponse } from 'src/app/_Models/Audit/auditdashboardmodel';
import { Auditsummarymodel } from 'src/app/_Models/Audit/auditsummarymodel';

@Injectable({
  providedIn: 'root'
})
export class AuditDashboardService {
  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }

  getAuditCountryGeoCode(request: Auditsummarymodel) {
    return this.http.post<any>(`${this.url}/api/auditdashboard/getauditcountrygeocode`, request)
      .pipe(map(response => { return response; }));
  }

  async getAuditDashboardSummary(request: Auditsummarymodel): Promise<AuditDashboardResponse> {
    return await this.http.post<any>(`${this.url}/api/auditdashboard/getauditdashboardsummary`, request).toPromise();
  }

  getServiceTypeAuditDashboardSummary(request: Auditsummarymodel) {
    return this.http.post<any>(`${this.url}/api/auditdashboard/getservicetypeauditdashboard`, request)
      .pipe(map(response => { return response }));
  }

  getAuditTypeAuditDashboard(request: Auditsummarymodel) {
    return this.http.post<any>(`${this.url}/api/auditdashboard/getaudittypeauditdashboard`, request)
      .pipe(map(response => { return response }));
  }

  getOverviewAuditDashboardSummary(request: Auditsummarymodel) {
    return this.http.post<any>(`${this.url}/api/auditdashboard/getoverviewdashboard`, request)
      .pipe(map(response => { return response }));
  }

  getServiceTypeChartExport(request: Auditsummarymodel) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/AuditDashboard/servicetypeexport`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

  getAuditTypeExport(request: Auditsummarymodel) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/auditdashboard/audittypeexport`, request, { headers: headers, responseType: 'blob' as 'json' });
  }
}
