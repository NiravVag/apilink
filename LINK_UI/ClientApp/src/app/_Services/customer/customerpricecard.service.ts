import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { DataSourceResponse } from 'src/app/_Models/common/common.model';
import {
  SaveCustomerPriceCardResponse, CustomerPriceCard, EditCustomerPriceCardResponse,
  CustomerPriceCardSummaryResponse, CustomerPriceCardSummary, CustomerPriceCardResponseResult,SamplingUnitPriceRequest,SamplingUnitPriceResponse,
  QuotationCustomerPriceCard, CustomerPriceCardRequest, CustomerPriceCardUnitPriceRequest, CustomerPriceCardUnitPriceResponse, QuotationPriceCard
} from 'src/app/_Models/customer/customer-price-card.model';

@Injectable({ providedIn: 'root' })

export class CustomerPriceCardService {

  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }
  getServiceTypesList() {
    return this.http.get<DataSourceResponse>(`${this.url}/api/Reference/ServiceTypeList`);
  }
  getBillingMethodList() {
    return this.http.get<DataSourceResponse>(`${this.url}/api/Reference/BillingMethodList`);
  }
  getBillingToList() {
    return this.http.get<DataSourceResponse>(`${this.url}/api/Reference/BillingToList`);
  }
  async save(model: CustomerPriceCard): Promise<SaveCustomerPriceCardResponse> {
    return await this.http.post<SaveCustomerPriceCardResponse>(`${this.url}/api/CustomerPriceCard`, model).toPromise();
  }
  async edit(id: number): Promise<EditCustomerPriceCardResponse> {
    return await this.http.get<EditCustomerPriceCardResponse>(`${this.url}/api/CustomerPriceCard/${id}`).toPromise();
  }
  async summaryData(model: CustomerPriceCardSummary): Promise<CustomerPriceCardSummaryResponse> {
    return await this.http.post<CustomerPriceCardSummaryResponse>(`${this.url}/api/CustomerPriceCard/getData`, model).toPromise();
  }
  async delete(id: number): Promise<CustomerPriceCardResponseResult> {
    return await this.http.get<CustomerPriceCardResponseResult>(`${this.url}/api/CustomerPriceCard/delete/${id}`).toPromise();
  }
  async getPriceCardData(request: CustomerPriceCardRequest): Promise<QuotationPriceCard> {
    return await this.http.post<QuotationPriceCard>(`${this.url}/api/Quotation/getPriceCardData`, request).toPromise();
  }
  async getUnitPriceByCustomerPriceCardRule(request: CustomerPriceCardUnitPriceRequest): Promise<CustomerPriceCardUnitPriceResponse> {
    return await this.http.post<CustomerPriceCardUnitPriceResponse>(`${this.url}/api/Quotation/getUnitPrice`, request).toPromise();
  }
  async getSamplingUnitPriceData(request: Array<SamplingUnitPriceRequest>): Promise<SamplingUnitPriceResponse> {
    return await this.http.post<SamplingUnitPriceResponse>(`${this.url}/api/Quotation/getSamplingUnitPrice`, request).toPromise();
  }
  exportSummary(model: CustomerPriceCardSummary) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/CustomerPriceCard/exportData`, model, { headers: headers, responseType: 'blob' as 'json' });
  }
  getCustomerPriceHolidayList() {
    return this.http.get<any>(`${this.url}/api/CustomerPriceCard/getcustomerpriceholidaylist`);
  }
}