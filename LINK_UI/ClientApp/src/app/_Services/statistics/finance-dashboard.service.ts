import { Injectable, Inject } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { promise } from "protractor";
import { MandayDashboardRequest } from "src/app/_Models/statistics/manday-dashboard.model";
import { map } from "rxjs/operators";
import { CommonDataSourceRequest } from "src/app/_Models/common/common.model";
import { FinanceDashboardRequestModel } from "src/app/_Models/statistics/finance-dashboard.model";

@Injectable({
  providedIn: 'root'
})
export class FinanceDashboardService {

  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }

  getBilledMandayData(request: FinanceDashboardRequestModel) {
    return this.http.post<any>(`${this.url}/api/FinanceDashboard/getBilledManday`, request)
      .pipe(map(response => {
        return response;
      }));
  }
  getMandayRateData(request: FinanceDashboardRequestModel) {
    return this.http.post<any>(`${this.url}/api/FinanceDashboard/getMandayRate`, request)
      .pipe(map(response => {
        return response;
      }));
  }

  async getBookingData(request: FinanceDashboardRequestModel) {
    return await this.http.post<any>(`${this.url}/api/FinanceDashboard/get-Booking-Details`, request).toPromise();
  }

  
  getRatioAnalysisData(request: FinanceDashboardRequestModel) {
    return this.http.post<any>(`${this.url}/api/FinanceDashboard/get-RationAnalysis-Details`, request);
  }

  getFinanceDashboardTurnOverData(bookingIdlist: Array<number>) {
    return this.http.post<any>(`${this.url}/api/FinanceDashboard/get-Turnover-Chart-Details`, bookingIdlist)
      .pipe(map(response => {
        return response;
      }));
  }

  getChargeBackData(bookingIdlist: Array<number>) {
    return this.http.post<any>(`${this.url}/api/FinanceDashboard/get-ChargeBack-Chart-Details`, bookingIdlist)
      .pipe(map(response => {
        return response;
      }));
  }

  getQuotationData(bookingIdlist: Array<number>) {
    return this.http.post<any>(`${this.url}/api/FinanceDashboard/get-Quotation-Chart-Details`, bookingIdlist)
      .pipe(map(response => {
        return response;
      }));
  }

  getBilledMandayChartExport(request: FinanceDashboardRequestModel) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/FinanceDashboard/BilledMandaychartExport`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

  getCountryTurnoverChartExport(request: FinanceDashboardRequestModel) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/FinanceDashboard/CountrychartExport`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

  getProdCategoryTurnoverChartExport(request: FinanceDashboardRequestModel) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/FinanceDashboard/ProdCategorychartExport`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

  getServiceTypeTurnoverChartExport(request: FinanceDashboardRequestModel) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/FinanceDashboard/ServiceTypechartExport`, request, { headers: headers, responseType: 'blob' as 'json' });
  }
  exportRatioAnalysis(request: FinanceDashboardRequestModel) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/FinanceDashboard/RatioAnalysisExport`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

  getEmployeeTypes(){
    return this.http.get<any>(`${this.url}/api/FinanceDashboard/get-employee-types`)
    .pipe(map(res => {
      return res;
    }));
  }
}  