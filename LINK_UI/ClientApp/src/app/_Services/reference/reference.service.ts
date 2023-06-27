import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CustomSampleSizeResponse } from 'src/app/_Models/reference/custom-sample-size.model';
import { map } from 'rxjs/operators';
import { CommonDataSourceRequest } from 'src/app/_Models/common/common.model';
import { EntityFeature } from 'src/app/components/common/static-data-common';


@Injectable({ providedIn: 'root' })

export class ReferenceService {

  url: string

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;
    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }

  // get custom sample size list
  getCustomSampleSizeList() {
    return this.http.get<CustomSampleSizeResponse>(`${this.url}/api/Reference/CustomSampleSize`);
  }

  getAPIServices() {
    return this.http.get<any>(`${this.url}/api/Reference/getapiservices`);
  }

  getCurrencyList() {
    return this.http.get<any>(`${this.url}/api/Reference/getCurrencyList`);
  }

  getCurrencyListWithCode() {
    return this.http.get<any>(`${this.url}/api/Reference/getCurrencyListWithCode`);
  }

  getBillingEntityList() {
    return this.http.get<any>(`${this.url}/api/Reference/getbillingentityList`);
  }

  getInvoiceRequestTypeList() {
    return this.http.get<any>(`${this.url}/api/Reference/getInvoiceRequestTypeList`);
  }

  checkUserHasInvoiceAccess() {
    return this.http.get<any>(`${this.url}/api/Reference/CheckUserHasInvoiceAccess`);
  }

  getInvoiceBankList(billingEntity) {
    return this.http.get<any>(`${this.url}/api/Reference/getInvoiceBankList/${billingEntity}`);
  }

  getBankList(billingEntity) {
    return this.http.get<any>(`${this.url}/api/Reference/getBankList/${billingEntity}`);
  }



  getInvoiceFeesTypeList() {
    return this.http.get<any>(`${this.url}/api/Reference/getInvoiceFeesTypeList`);
  }

  getInvoiceOfficeList() {
    return this.http.get<any>(`${this.url}/api/Reference/getInvoiceOfficeList`);
  }

  getInvoicePaymentTypeList() {
    return this.http.get<any>(`${this.url}/api/Reference/getInvoicePaymentTypeList`);
  }

  getServiceTypelist(customerId: number, serviceId: number) {
    return this.http.get<any>(`${this.url}/api/reference/serviceType-list-cus-service/${customerId}/${serviceId}`);
  }

  getInvoiceExtraTypeList() {
    return this.http.get<any>(`${this.url}/api/Reference/getInvoiceExtraTypeList`);
  }

  getFullBridgeResultData() {
    return this.http.get<any>(`${this.url}/api/Reference/get-full-bridge-result-data`);
  }

  getServiceList() {
    return this.http.get<any>(`${this.url}/api/Reference/get-service-data`);
  }

  getOfficeList() {
    return this.http.get<any>(`${this.url}/api/Reference/get-office-locations`);
  }

  getServiceTypeList() {
    return this.http.get<any>(`${this.url}/api/Reference/ServiceTypeList`);
  }

  getDelimiterList() {
    return this.http.get<any>(`${this.url}/api/Reference/get-delimiter-data`);
  }

  getOfficeLocationList() {
    return this.http.get<any>(`${this.url}/api/InspectionBooking/get-office-list`);
  }

  getInspectionLocationList() {
    return this.http.get<any>(`${this.url}/api/reference/get-inspection-locations`);
  }

  getInspectionShipmentTypes() {
    return this.http.get<any>(`${this.url}/api/reference/get-inspection-shipmenttypes`);
  }

  getBusinessLines() {
    return this.http.get<any>(`${this.url}/api/reference/get-business-lines`);
  }

  getSeasonYear() {
    return this.http.get<any>(`${this.url}/api/reference/get-business-lines`);
  }

  getServiceTypes(request) {
    return this.http.post<any>(`${this.url}/api/reference/get-service-types`, request)
      .pipe(map(response => {
        return response;
      }));
  }


  getEntityList() {
    return this.http.get<any>(`${this.url}/api/Reference/getEntityList`);
  }

  getTripTypeList() {
    return this.http.get<any>(`${this.url}/api/Reference/GetTripTypeList`);
  }

  getUserEntityList(userType, id) {
    return this.http.get<any>(`${this.url}/api/Reference/getuserEntityList/${userType}/${id}`);
  }

  getServiceTypeListByServiceIds(serviceIds: number[]) {
    return this.http
      .post<any>(`${this.url}/api/Reference/ServiceTypeList`, serviceIds)
      .pipe(
        map((response) => {
          return response;
        })
      );
  }


  getBillFrequencyList() {
    return this.http.get<any>(`${this.url}/api/reference/GetBillFrequencyList`);
  }

  getBillQuantityTypeList() {
    return this.http.get<any>(`${this.url}/api/reference/GetBillQuantityTypeList`);
  }

  getInterventionTypeList() {
    return this.http.get<any>(`${this.url}/api/reference/GetInterventionTypeList`);
  }

  getEntityFeatureList() {
    return this.http.get<any>(`${this.url}/api/reference/GetEntityFeatureList`);
  }



  getStaffList(model: CommonDataSourceRequest, term?: string) {
    model.searchText = "";
    if (term) {
      model.searchText = term;
      model.idList = null;
    }
    return this.http.post<any>(`${this.url}/api/reference/GetStaffDataSourceList`, model)
      .pipe(map(response => {
        console.log(" ***** response", response);
        return response.dataSourceList;
      }));
  }


  getExpertiseList() {
    return this.http.get<any>(`${this.url}/api/reference/GetExpertiseList`);
  }

  getInspectionBookingTypes() 
  {
    return this.http.get<any>(`${this.url}/api/reference/get-inspection-booking-types`);
  }

  getInspectionPaymentOptions(customerId) 
  {
    return this.http.get<any>(`${this.url}/api/reference/get-inspection-payment-options/${customerId}`);
  }


  isEntityFeatureExist(featureId: EntityFeature) {
    return this.http.get<any>(`${this.url}/api/reference/isEntityFeatureExist/${featureId}`);
  }

  getAuditServiceTypeList() 
  {
    return this.http.get<any>(`${this.url}/api/reference/get-audit-service-list/`);
  }

}
