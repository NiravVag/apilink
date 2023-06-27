import { Injectable, Inject } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { promise } from "protractor";
import { MandayDashboardRequest } from "src/app/_Models/statistics/manday-dashboard.model";
import { map } from "rxjs/operators";
import { BookingReportResponse, CountryDefectChartResponse, DefectCategoryResponse, DefectDashboardModel, DefectPerformanceAnalysis, DefectPerformanceResponse, DefectYearCountResponse, DefectYearInnerCountResponse, ParetoDefectResponse, ReportDefectResponse } from "src/app/_Models/statistics/defect-dashboard.model";
import { CommonDataSourceRequest } from "src/app/_Models/common/common.model";

@Injectable({
  providedIn: 'root'
})
export class DefectDashboardService {

  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }

  getBookingReportDetails(request: DefectDashboardModel) {
    return this.http.post<BookingReportResponse>(`${this.url}/api/DefectDashboard/get-booking-details`, request)
      .pipe(map(response => {
        return response;
      }));
  }

  getDefectCategoryList(request: DefectDashboardModel) {
    return this.http.post<DefectCategoryResponse>(`${this.url}/api/DefectDashboard/get-defect-category-details`, request)
      .pipe(map(response => {
        return response;
      }));
  }

  getDefectCategoryListChartExport(request: DefectDashboardModel) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/DefectDashboard/get-defect-category-export`,
      request, { headers: headers, responseType: 'blob' as 'json' });
  }

  // getDefectYearCountList(reportIdList: Array<number>) {
  //   return this.http.post<DefectYearCountResponse>(`${this.url}/api/DefectDashboard/get-defect-year-details`, reportIdList)
  //     .pipe(map(response => {
  //       return response;
  //     }));
  // }

  getDefectYearCountExport(request: DefectDashboardModel) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/DefectDashboard/get-defect-year-count-export`,
      request, { headers: headers, responseType: 'blob' as 'json' });
  }

  getBookingChangeYearDetails(request: DefectDashboardModel) {
    return this.http.post<DefectYearInnerCountResponse>(`${this.url}/api/DefectDashboard/get-defect-year-count-details`, request)
      .pipe(map(response => {
        return response;
      }));
  }

  getParetoDefectList(request: DefectDashboardModel) {
    return this.http.post<ParetoDefectResponse>(`${this.url}/api/DefectDashboard/get-pareto-defect-list`, request)
      .pipe(map(response => {
        return response;
      }));
  }

  getParetoDefectListExport(request: DefectDashboardModel) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/DefectDashboard/get-pareto-defect-list-export`,
      request, { headers: headers, responseType: 'blob' as 'json' });
  }

  getLowPerformanceDefectList(model: DefectPerformanceAnalysis) {
    return this.http.post<DefectPerformanceResponse>(`${this.url}/api/DefectDashboard/get-low-performance-defect-list`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  getDefectDataSourceList(model: CommonDataSourceRequest, term?: string) {
    model.searchText = term ? term : "";
    return this.http.post<any>(`${this.url}/api/DefectDashboard/get-defect-list`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }
  getLowPerformanceExport(model: DefectPerformanceAnalysis) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/DefectDashboard/get-defect-low-performance-export`,
      model, { headers: headers, responseType: 'blob' as 'json' });
  }

  getLowPerformanceDefectCountList(model: DefectPerformanceAnalysis) {
    return this.http.post<any>(`${this.url}/api/DefectDashboard/get-defect-count-list`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  getLowPerformanceDefectPhotoList(model: DefectPerformanceAnalysis) {
    return this.http.post<any>(`${this.url}/api/DefectDashboard/get-defect-photo-list`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  getCountryDefectListExport(request: DefectDashboardModel) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/DefectDashboard/get-country-defect-export`,
      request, { headers: headers, responseType: 'blob' as 'json' });
  }

  getCountryDefectList(request: DefectDashboardModel) {
    return this.http.post<CountryDefectChartResponse>(`${this.url}/api/DefectDashboard/get-country-defect-list`, request)
      .pipe(map(response => {
        return response;
      }));
  }

  getReportDefectPareto(request: DefectDashboardModel) {
    return this.http.post<any>(`${this.url}/api/DefectDashboard/getReportDefectPareto`, request)
      .pipe(map(response => { return response }));
  }

  exportReportDefectPareto(request: DefectDashboardModel){
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/DefectDashboard/export-report-defect-pareto`, request, { headers: headers, responseType: 'blob' as 'json' });
  }
}  