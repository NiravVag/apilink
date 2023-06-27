import { Injectable, Inject } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { MandayDashboardRequest } from "src/app/_Models/statistics/manday-dashboard.model";
import { map } from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class MandayUtilisationDashboardService {

  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }
  
  getmandayByUtilisation(request:MandayDashboardRequest) {
    return this.http.post<any>(`${this.url}/api/UtilizationDashboard/Search`,request)
    .pipe(map(response => {
      return response;
    }));
  }

  getMandayUtilisationExport(request: MandayDashboardRequest) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/UtilizationDashboard/ExportUtilizationDashboard`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

  
  getmandayByYear(request:MandayDashboardRequest) {
    return this.http.post<any>(`${this.url}/api/UtilizationDashboard/ManDayYearChart`,request)
    .pipe(map(response => {
      return response;
    }));
  }  

  getMandayByYearExport(request: MandayDashboardRequest) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/UtilizationDashboard/ManDayYearChartExport`, request, { headers: headers, responseType: 'blob' as 'json' });
  }
}