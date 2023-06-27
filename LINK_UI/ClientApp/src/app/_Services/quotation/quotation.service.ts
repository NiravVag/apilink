import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, first } from 'rxjs/operators';
import {
  QuotationResponse, QuotationDataSourceResponse, QuotationEntityType, QuotationContactListResponse, OrderListResponse,
  FilterOrderRequest, QuotationModel, SaveQuotationResponse, AddressFactoryResponse, SetStatusRequest, SetStatusQuotationResponse, QuotationManDayResponse, QuoationMandayrequest, QuotProduct, QuotCheckpointRequest, CalculatedWorkingHoursReponse, PriceCardTravelResponse, FactoryBookingInfoRequest,
} from '../../_Models/quotation/quotation.model';
import { QuotationSummaryResponse, QuotationDataSummaryResponse, Quotationsummarymodel} from 'src/app/_Models/quotation/quotationsummary.model';
import { QuotationCustomerPriceCard } from 'src/app/_Models/customer/customer-price-card.model';
import { InvoiceModel } from '../../_Models/quotation/quotationsummary.model';

@Injectable({ providedIn: 'root' })

export class QuotationService {

  url : string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }


  async getQuotation(id: number): Promise<QuotationResponse> {
    if(id == null)
      return await this.http.get<QuotationResponse>(`${this.url}/api/quotation`).toPromise();

    return await this.http.get<QuotationResponse>(`${this.url}/api/quotation/${id}`).toPromise();
  }

  GetPODetailsByBookingAndProduct(bookingId,productRefId)
  {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/GetBookingPoListbyProduct/${bookingId}/${productRefId}`);
  }

  async getCustomerList(countryId: number, serviceId: number): Promise<QuotationDataSourceResponse>{
    return await this.http.get<QuotationDataSourceResponse>(`${this.url}/api/quotation/customer-list/${countryId}/${serviceId}`).toPromise();
  }
  async getSupplierList(customerId: number): Promise<QuotationDataSourceResponse> {
    return await this.http.get<QuotationDataSourceResponse>(`${this.url}/api/quotation/supplier-list/${customerId}`).toPromise();
  }

  async getFactoryList(supplierId: number): Promise<QuotationDataSourceResponse> {
    return await this.http.get<QuotationDataSourceResponse>(`${this.url}/api/quotation/factory-list/${supplierId}`).toPromise();
  }

  async getContactList(id: number, type: QuotationEntityType, parentId:number): Promise<QuotationContactListResponse> {
    switch (type) {
      case QuotationEntityType.Customer:
        return await this.http.get<QuotationContactListResponse>(`${this.url}/api/quotation/cust-contact-list/${id}`).toPromise();
      case QuotationEntityType.Supplier:
        return await this.http.get<QuotationContactListResponse>(`${this.url}/api/quotation/supp-contact-list/${id}/${parentId}`).toPromise();
      case QuotationEntityType.Factory:
        return await this.http.get<QuotationContactListResponse>(`${this.url}/api/quotation/fact-contact-list/${id}/${parentId}`).toPromise();
      case QuotationEntityType.Internal:
        return await this.http.get<QuotationContactListResponse>(`${this.url}/api/quotation/intern-contact-list/${id}/${parentId}`).toPromise();
    }

    return null;

  }

  async getOrderList(request: FilterOrderRequest): Promise<OrderListResponse> {
    return await this.http.post<OrderListResponse>(`${this.url}/api/quotation/order-list`, request).toPromise();
  }

  async getQuotationSummary(): Promise<QuotationSummaryResponse> {
    return await this.http.get<QuotationSummaryResponse>(`${this.url}/api/quotation/quotation-summary`).toPromise();
  }

  async getQuotationDataSummary(request: Quotationsummarymodel): Promise<QuotationDataSummaryResponse> {
    return await this.http.post<QuotationDataSummaryResponse>(`${this.url}/api/quotation/quotation-list`, request).toPromise();
  }

  async saveQuotation(model: QuotationModel): Promise<SaveQuotationResponse> {
    return await this.http.put<SaveQuotationResponse>(`${this.url}/api/quotation`, model).toPromise();
  }

  async getAddressFactory(factoryId: number): Promise<AddressFactoryResponse> {
    return await this.http.get<AddressFactoryResponse>(`${this.url}/api/quotation/factory-address/${factoryId}`).toPromise();
  }

  async setStatus(request: SetStatusRequest): Promise<SetStatusQuotationResponse> {
    return await this.http.post<SetStatusQuotationResponse>(`${this.url}/api/quotation/status`, request).toPromise();
  }
  async getQuotaionManday(request: QuoationMandayrequest): Promise<QuotationManDayResponse> {
    return await this.http.post<QuotationManDayResponse>(`${this.url}/api/quotation/quotation-manday`, request).toPromise();
  }
  preview(id: number) {
    // return this.http.get<any>(`${this.url}/api/quotation/preview/${id}`);

    return this.http.get<any>(`${this.url}/api/quotation/preview/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }

  getVersion(id: string) {
    return this.http.get(`${this.url}/api/quotation/version/${id}`, { responseType: 'blob' });
  }
  exportSummary(request: Quotationsummarymodel) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/quotation/export-quotation`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

  async getServiceTypelist(customerId: number, serviceId: number): Promise<QuotationDataSourceResponse> {
    return await this.http.get<QuotationDataSourceResponse>(`${this.url}/api/reference/serviceType-list/${customerId}/${serviceId}`).toPromise();
  }
  async checkQuotationSampleQtyAndBookingSampleQtyAreEqual(quotProducts: Array<QuotProduct>): Promise<boolean> {
    return await this.http.post<boolean>(`${this.url}/api/quotation/quotation-sampleqty`, quotProducts).toPromise();
  }

  getClientQuotation(request) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/quotation/clientQuotation`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

  saveInvoice(model: InvoiceModel) {
    return this.http.post<any>(`${this.url}/api/quotation/invoiceSave`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  getInvoice(quotationId: number, serviceId: number) {
    return this.http.get<any>(`${this.url}/api/quotation/getinvoice/${quotationId}/${serviceId}`)
      .pipe(map(response => {
        return response;
      }));
  }

  getBillPaidByist() {
    return this.http.get<any>(`${this.url}/api/quotation/getBillPaidByList`)
      .pipe(map(response => {
        return response;
      }));
  }

  getQuotationStatusColor() {
    return this.http.get<any>(`${this.url}/api/quotation/getQuotationStatusColor`)
      .pipe(map(response => {
        return response;
      }));
  }

  async getSkipQuotationSentToClient(request: QuotCheckpointRequest) : Promise<boolean> {
    return await this.http.post<boolean>(`${this.url}/api/quotation/getSkipQuotationSentToClientCheckpoint`, request).toPromise();
  }

  async getCalculatedWorkingHours(id: number): Promise<CalculatedWorkingHoursReponse> {
    return await this.http.get<CalculatedWorkingHoursReponse>(`${this.url}/api/quotation/get-Calculated_working-manday/${id}`).toPromise();
  }
  async getPriceCardTravel(ruleId: number): Promise<PriceCardTravelResponse> {
    return await this.http.get<PriceCardTravelResponse>(`${this.url}/api/quotation/get-price-card-travel/${ruleId}`).toPromise();
  }
  async saveWorkingManday(bookingId: number): Promise<CalculatedWorkingHoursReponse> {
    return await this.http.get<CalculatedWorkingHoursReponse>(`${this.url}/api/quotation/save-working-manday/${bookingId}`).toPromise();
  }

  factoryBookingInfo(request: FactoryBookingInfoRequest) {
    return this.http.post<any>(`${this.url}/api/Quotation/factory-booking-info`, request);
  }
}
