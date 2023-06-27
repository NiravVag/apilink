import { Injectable, inject, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { ProductCategoryRequest, QuantitativeDashboardRequest, QuantitativeDashboardSummaryResponse } from '../../_Models/statistics/quantitativedashboard.model';
import { ManagementDashboardRequest } from 'src/app/_Models/dashboard/managementdashboard.model';

@Injectable({
  providedIn: 'root'
})
export class QuantitativeDashBoardService {
  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }

  async getDashboardSummary(model: QuantitativeDashboardRequest): Promise<QuantitativeDashboardSummaryResponse> {
    return await this.http.post<any>(`${this.url}/api/QuantitativeDashboard/getQuantitativeDashboardSummary`, model).toPromise();
  }

  getManDayDashboardSummary(model: QuantitativeDashboardRequest) {
    return this.http.post<any>(`${this.url}/api/QuantitativeDashboard/get-manday-count-by-year`, model)
      .pipe(map(response => { return response }));
  }

  getMandayYearChartExport(request: QuantitativeDashboardRequest) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/QuantitativeDashboard/MandaychartExport`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

  getManDayByCountrySummary(model: QuantitativeDashboardRequest) {
    return this.http.post<any>(`${this.url}/api/QuantitativeDashboard/MandayByCountry`, model)
      .pipe(map(response => { return response }));
  }

  getMandayCountryChartExport(request: QuantitativeDashboardRequest) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/QuantitativeDashboard/MandayCountrychartExport`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

  getTurnOverSummary(model: QuantitativeDashboardRequest) {
    return this.http.post<any>(`${this.url}/api/QuantitativeDashboard/TurnOverSummary`, model)
      .pipe(map(response => { return response }));
  }

  getTurnOverByServicetypeChartExport(request: QuantitativeDashboardRequest) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/QuantitativeDashboard/TurnOverServiceTypechartExport`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

  getInspectionServiceTypeDashboardSummary(model: QuantitativeDashboardRequest) {
    return this.http.post<any>(`${this.url}/api/QuantitativeDashboard/getInspectionServiceTypeDashboard`, model)
      .pipe(map(response => { return response }));
  }

  getInspectionByServicetypeChartExport(request: QuantitativeDashboardRequest) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/QuantitativeDashboard/InspectionServiceTypechartExport`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

  getOrderQuantityList(request: QuantitativeDashboardRequest) {
    return this.http.post<any>(`${this.url}/api/QuantitativeDashboard/get-order-quantity-list`, request)
      .pipe(map(response => { return response }));
  }

  getOrderQuantityListExport(request: QuantitativeDashboardRequest) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/QuantitativeDashboard/get-order-quantity-list-export`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

  getProductCategoryCountListExport(request: QuantitativeDashboardRequest) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/QuantitativeDashboard/get-product-category-list-export`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

  getProductCategoryCountList(request: ProductCategoryRequest) {
    return this.http.post<any>(`${this.url}/api/QuantitativeDashboard/get-product-category-list`, request)
      .pipe(map(response => { return response }));
  }

  getProductCategorySummary() {
    return this.http.get<any>(`${this.url}/api/QuantitativeDashboard/productcategory`)
      .pipe(map(Response => { return Response; }));
  }

}