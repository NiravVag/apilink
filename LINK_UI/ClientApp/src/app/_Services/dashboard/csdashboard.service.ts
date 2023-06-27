import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import {ManagementDashboardRequest, ManagementDashboardSummaryResponse} from '../../_Models/dashboard/managementdashboard.model';
import { CSDashboardRequest } from 'src/app/_Models/dashboard/csdashboard.model';

@Injectable({
    providedIn: 'root'
})
export class CSDashBoardService {
    url: string
    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.url = baseUrl;

        if (this.url.charAt(this.url.length - 1) == '/')
            this.url = this.url.substring(0, this.url.length - 1);
    }

    async getDashboardSummary(model: ManagementDashboardRequest): Promise<ManagementDashboardSummaryResponse> {
        return await this.http.post<any>(`${this.url}/api/ManagementDashboard/getMandayDashboardSummary`, model).toPromise();
    }

    getCountNewBookingRelatedDetails(model: CSDashboardRequest) {
        return this.http.post<any>(`${this.url}/api/CSDashboard/get-new-data-count`, model)
            .pipe(map(response => { return response }));
    }

    getInspectionServiceTypeList(model: CSDashboardRequest) {
        return this.http.post<any>(`${this.url}/api/CSDashboard/get-service-type-details`, model)
            .pipe(map(response => { return response }));
    }

    getInspectionMandayByOfficeList(model: CSDashboardRequest) {
        return this.http.post<any>(`${this.url}/api/CSDashboard/get-manday-by-office-details`, model)
            .pipe(map(response => { return response }));
    }

    getTotalReportsCountByDayList(model: CSDashboardRequest) {
        return this.http.post<any>(`${this.url}/api/CSDashboard/get-report-count-by-day-details`, model)
            .pipe(map(response => { return response }));
    }

    getStatusByLoggedUserList(model: CSDashboardRequest) {
        return this.http.post<any>(`${this.url}/api/CSDashboard/get-status-count-by-logged-user-details`, model)
            .pipe(map(response => { return response }));
    }

    // getProductCategoryChartExport(request: ManagementDashboardRequest) {
    //     const headers = new HttpHeaders({
    //       'Content-Type': 'application/json',
    //       'Accept': 'application/json'
    //     });
    //     return this.http.post<Blob>(`${this.url}/api/ManagementDashboard/ProductCategoryExport`, request, { headers: headers, responseType: 'blob' as 'json' });
    //   }
}