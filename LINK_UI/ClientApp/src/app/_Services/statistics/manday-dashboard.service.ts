import { Injectable, Inject } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { promise } from "protractor";
import { MandayDashboardResponse, MandayDashboardRequest } from "src/app/_Models/statistics/manday-dashboard.model";
import { map } from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class MandayDashboardService {

  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }
  
  getCountryListByName(name: string) {
    return this.http.get<any>(`${this.url}/api/Location/GetCountries/${name}`)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }

  getCustomerListByName(name: string) {
    return this.http.get<any>(`${this.url}/api/Customer/GetCustomerByName/${name}`)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }

  getServiceList() {
    return this.http.get<any>(`${this.url}/api/Manday/GetService`)
      .pipe(map(response => {
        return response;
      }));
  }
  getOfficeList() {
    return this.http.get<any>(`${this.url}/api/Manday/GetOfficeLocations`)
      .pipe(map(response => {
        return response;
      }));
  }
  getmandayByYear(request:MandayDashboardRequest) {
    return this.http.post<any>(`${this.url}/api/Manday/ManDayYearChart`,request)
    .pipe(map(response => {
      return response;
    }));
  }

  getmandayByCustomerChart(request:MandayDashboardRequest) {
    return this.http.post<any>(`${this.url}/api/Manday/ManDayCustomerChart`,request)
    .pipe(map(response => {
      return response;
    }));
  }

  getmandayByCountryChart(request:MandayDashboardRequest) {
    return this.http.post<any>(`${this.url}/api/Manday/ManDayCountryChart`,request)
    .pipe(map(response => {
      return response;
    }));
  }

  getmandayByEmployeeTypeChart(request:MandayDashboardRequest) {
    return this.http.post<any>(`${this.url}/api/Manday/ManDayEmployeeTypeChart`,request)
    .pipe(map(response => {
      return response;
    }));
  }

  getMandayDashboardSearch(request:MandayDashboardRequest) {
    return this.http.post<any>(`${this.url}/api/Manday/Search`, request)
      .pipe(map(response => {
        return response;
      }));
  }

  getMandayByYearExport(request: MandayDashboardRequest) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/Manday/ManDayYearChartExport`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

  getManDayCustomerChartExport(request: MandayDashboardRequest) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/Manday/ManDayCustomerChartExport`, request, { headers: headers, responseType: 'blob' as 'json' });
  }
  getManDayCountryChartExport(request: MandayDashboardRequest) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/Manday/ManDayCountryChartExport`, request, { headers: headers, responseType: 'blob' as 'json' });
  }
  getManDayEmployeeTypeChartExport(request: MandayDashboardRequest) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/Manday/ManDayEmployeeTypeChartExport`, request, { headers: headers, responseType: 'blob' as 'json' });
  }
}  