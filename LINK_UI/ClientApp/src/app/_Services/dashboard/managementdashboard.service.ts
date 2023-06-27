import { Injectable, inject, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import {ManagementDashboardRequest, ManagementDashboardSummaryResponse} from '../../_Models/dashboard/managementdashboard.model';

@Injectable({
    providedIn: 'root'
})
export class ManagementDashBoardService {
    url: string
    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.url = baseUrl;

        if (this.url.charAt(this.url.length - 1) == '/')
            this.url = this.url.substring(0, this.url.length - 1);
    }

    async getDashboardSummary(model: ManagementDashboardRequest): Promise<ManagementDashboardSummaryResponse> {
        return await this.http.post<any>(`${this.url}/api/ManagementDashboard/getMandayDashboardSummary`, model).toPromise();
    }

    getOverviewDashboardSummary(request: ManagementDashboardRequest) {
        return this.http.post<any>(`${this.url}/api/ManagementDashboard/getOverviewDashboard`, request)
        .pipe( map ( response => { return response }));
    }

    getRejectDashboardSummary(request: ManagementDashboardRequest) {
        return this.http.post<any>(`${this.url}/api/ManagementDashboard/getRejectDashboard`, request)
        .pipe( map ( response => { return response }));
    }

    getProductCategoryDashboardSummary(request: ManagementDashboardRequest) {
        return this.http.post<any>(`${this.url}/api/ManagementDashboard/getProductCategoryDashboard`, request)
        .pipe( map ( response => { return response }));
    }

    getResultDashboardSummary(request: ManagementDashboardRequest) {
        return this.http.post<any>(`${this.url}/api/ManagementDashboard/getResultDashboard`, request)
        .pipe( map ( response => { return response }));
    }

    getServiceTypeDashboardSummary(request: ManagementDashboardRequest) {
        return this.http.post<any>(`${this.url}/api/ManagementDashboard/getServiceTypeDashboard`, request)
        .pipe( map ( response => { return response }));
    }

    getManDayDashboardSummary(request: ManagementDashboardRequest) {
        return this.http.post<any>(`${this.url}/api/ManagementDashboard/get-manday-count-by-year`, request)
        .pipe( map ( response => { return response }));
    }

    getAverageBookingStatusChangeDashboardSummary(request: ManagementDashboardRequest) {
        return this.http.post<any>(`${this.url}/api/ManagementDashboard/getAverageBookingTimeDashboard`, request)
        .pipe( map ( response => { return response }));
    }

    getAverageQuotationStatusChangeDashboardSummary(request: ManagementDashboardRequest) {
        return this.http.post<any>(`${this.url}/api/ManagementDashboard/getAverageQuotationTimeDashboard`, request)
        .pipe( map ( response => { return response }));
    }

    getProductCategoryChartExport(request: ManagementDashboardRequest) {
        const headers = new HttpHeaders({
          'Content-Type': 'application/json',
          'Accept': 'application/json'
        });
        return this.http.post<Blob>(`${this.url}/api/ManagementDashboard/ProductCategoryExport`, request, { headers: headers, responseType: 'blob' as 'json' });
      }

    getServiceTypeChartExport(request: ManagementDashboardRequest) {
    const headers = new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/ManagementDashboard/ServiceTypeExport`, request, { headers: headers, responseType: 'blob' as 'json' });
    }

    getResultChartExport(request: ManagementDashboardRequest) {
    const headers = new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/ManagementDashboard/ResultExport`, request, { headers: headers, responseType: 'blob' as 'json' });
    }
}