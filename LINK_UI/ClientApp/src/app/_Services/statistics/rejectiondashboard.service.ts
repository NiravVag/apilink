import { Injectable, inject, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { ManagementDashboardRequest } from 'src/app/_Models/dashboard/managementdashboard.model';
import { CountryChartRequest,RejectChartRequest, RejectionDashboardRequest, RejectionDashboardSummaryResponse,RejectChartSubcatogoryRequest ,RejectChartSubcatogory2Request, RejectionRateResponse} from 'src/app/_Models/statistics/rejectiondashborad.model';

@Injectable({
  providedIn: 'root'
})
export class RejectionDashBoardService {
  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }

  async getAPIResultDashboardSummary(model: RejectionDashboardRequest): Promise<RejectionDashboardSummaryResponse> {
    return await this.http.post<any>(`${this.url}/api/RejectionDashboard/getAPIResultDashboard`, model).toPromise();
  }

  getCustomerResultDashboardSummary(model: RejectionDashboardRequest) {
    return this.http.post<any>(`${this.url}/api/RejectionDashboard/getCustomerResultDashboard`, model)
      .pipe(map(response => { return response }));
  }

  getApiResultChartExport(request: RejectionDashboardRequest) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/RejectionDashboard/apiResultChartExport`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

  getCustomerResultChartExport(request: RejectionDashboardRequest) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/RejectionDashboard/customerResultChartExport`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

  getProductCategoryDashboardSummary(model: RejectionDashboardRequest) {
    return this.http.post<any>(`${this.url}/api/RejectionDashboard/getProductCategoryDashboard`, model)
      .pipe(map(response => { return response }));
  }

  getProductCategoryChartExport(request: RejectionDashboardRequest) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/RejectionDashboard/productCategoryChartExport`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

  getVendorDashboardSummary(model: RejectionDashboardRequest) {
    return this.http.post<any>(`${this.url}/api/RejectionDashboard/getVendorDashboard`, model)
      .pipe(map(response => { return response }));
  }

  getVendorChartExport(request: RejectionDashboardRequest) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/RejectionDashboard/vendorChartExport`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

  getCountryDashboardSummary(request: CountryChartRequest) {
    return this.http.post<any>(`${this.url}/api/RejectionDashboard/getCountryDashboard`, request)
      .pipe(map(response => { return response }));
  }

  getCountryChartExport(request: RejectionDashboardRequest) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/RejectionDashboard/countryChartExport`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

  getRejectDashboardSummary(request: RejectChartRequest) {
    return this.http.post<any>(`${this.url}/api/RejectionDashboard/getRejectDashboard`, request)
      .pipe(map(response => { return response }));
  }
  getRejectDashboardPopUpSummary(request: RejectionDashboardRequest) {
    return this.http.post<any>(`${this.url}/api/RejectionDashboard/getRejectDashboardPopUpData`, request)
      .pipe(map(response => { return response }));
  }
  
  getRejectImages(request: RejectionDashboardRequest) {
    return this.http.post<any>(`${this.url}/api/RejectionDashboard/getRejectImages`, request)
      .pipe(map(response => { return response }));
  }
  getRejectSubcatogorySummary(request: RejectChartSubcatogoryRequest) {
    return this.http.post<any>(`${this.url}/api/RejectionDashboard/getRejectSubCatogory`, request)
      .pipe(map(response => { return response }));
  }
  getRejectSubcatogory2Summary(request: RejectChartSubcatogory2Request) {
    return this.http.post<any>(`${this.url}/api/RejectionDashboard/getRejectSubCatogory2`, request)
      .pipe(map(response => { return response }));
  }

  exportRejectDashboardAnalysisTable(request: RejectChartSubcatogory2Request){
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/RejectionDashboard/exportRejectDashboard`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

  getReportRejectionRate(request: RejectionDashboardRequest) {
    return this.http.post<any>(`${this.url}/api/RejectionDashboard/getReportRejectionRate`,request)
      .pipe(map(response => { return response }));
  }
  
  exportReportRejectionRate(request: RejectionDashboardRequest){
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/RejectionDashboard/exportReportRejectionRate`, request, { headers: headers, responseType: 'blob' as 'json' });
  }
}
