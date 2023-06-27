import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, first } from 'rxjs/operators';
import { EditCustomerModel } from '../../_Models/customer/edit-customer.model';
import { CommonCustomerSourceRequest, CommonDataSourceRequest, CustomerDataSourceRequest, BuyerDataSourceRequest, CustomerContactSourceRequest, CustomerCommonDataSourceRequest } from 'src/app/_Models/common/common.model';
import { mode } from 'crypto-js';
import { CustomerKAMDetail } from 'src/app/_Models/user/customerkamdetails.model';
import { UserType } from 'src/app/components/common/static-data-common';

@Injectable({ providedIn: 'root' })
export class CustomerService {

  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }
  getCustomerSummary() {
    return this.http.get<any>(`${this.url}/api/customer`)
      .pipe(map(response => {
        return response;
      }));
  }
  getCustomerbyId(id: number) {
    return this.http.get<any>(`${this.url}/api/customer/getcustomerbyid/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }
  getCustomerGroup() {
    return this.http.get<any>(`${this.url}/api/customer/customergroup`)
      .pipe(map(response => {
        return response;
      }));
  }
  getLanguage() {
    return this.http.get<any>(`${this.url}/api/customer/language`)
      .pipe(map(response => {
        return response;
      }));
  }
  getProspectStatus() {
    return this.http.get<any>(`${this.url}/api/customer/prospectstatus`)
      .pipe(map(response => {
        return response;
      }));
  }
  getMarketSegment() {
    return this.http.get<any>(`${this.url}/api/customer/marketsegment`)
      .pipe(map(response => {
        return response;
      }));
  }
  getBusinessType() {
    return this.http.get<any>(`${this.url}/api/customer/businesstype`)
      .pipe(map(response => {
        return response;
      }));
  }
  getAddressType() {
    return this.http.get<any>(`${this.url}/api/customer/addresstype`)
      .pipe(map(response => {
        return response;
      }));
  }
  getInvoiceType() {
    return this.http.get<any>(`${this.url}/api/customer/invoicetype`)
      .pipe(map(response => {
        return response;
      }));
  }
  getAccountingLeader() {
    return this.http.get<any>(`${this.url}/api/customer/accountingleader`)
      .pipe(map(response => {
        return response;
      }));
  }
  getStaffKAMList() {
    return this.http.get<any>(`${this.url}/api/customer/getKAMStaffDetails`)
      .pipe(map(response => {
        return response;
      }));
  }
  getSalesIncharge(departname: string) {
    return this.http.get<any>(`${this.url}/api/customer/salesincharge/${departname}`)
      .pipe(map(response => {
        return response;
      }));
  }
  getActivitiesLevel() {
    return this.http.get<any>(`${this.url}/api/customer/activitieslevel`)
      .pipe(map(response => {
        return response;
      }));
  }
  getRelationshipStatus() {
    return this.http.get<any>(`${this.url}/api/customer/relationshipstatus`)
      .pipe(map(response => {
        return response;
      }));
  }
  getBrandPriority() {
    return this.http.get<any>(`${this.url}/api/customer/brandpriority`)
      .pipe(map(response => {
        return response;
      }));
  }
  getDataSummary(request) {
    return this.http.post<any>(`${this.url}/api/customer/search`, request)
      .pipe(map(response => {

        return response;
      }));
  }

  deleteCustomer(id: number) {
    return this.http.get<any>(`${this.url}/api/customer/delete/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }

  getEditCustomer(id) {

    if (id == null)
      return this.http.get<any>(`${this.url}/api/customer/add`);
    else
      return this.http.get<any>(`${this.url}/api/customer/edit/${id}`);
  }


  saveCustomer(model: EditCustomerModel) {
    return this.http.post<any>(`${this.url}/api/customer/save`, model)
      .pipe(map(response => {
        return response;
      }));
  }
  //get customer list by user type
  GetCustomerByUsertType() {
    return this.http.get<any>(`${this.url}/api/customer/GetCustomerByUsertType`)
      .pipe(map(response => {
        return response;
      }));
  }
  getCustomerBrands(customerId) {
    return this.http.get<any>(`${this.url}/api/customer/getcustomerbrands/${customerId}`);
  }
  getCustomerDepartments(customerId) {
    return this.http.get<any>(`${this.url}/api/customer/getcustomerdepartments/${customerId}`);
  }
  getCustomerBuyers(customerId) {
    return this.http.get<any>(`${this.url}/api/customer/getcustomerbuyers/${customerId}`);
  }
  getCustomerProductCategoryList(customerId) {
    return this.http.get<any>(`${this.url}/api/customer/getcustomerproductcategory/${customerId}`);
  }

  getCustomerProductSubCategoryList(requestCustomerSubCategory: any) {

    return this.http.post<any>(`${this.url}/api/customer/getcustomerproductsubcategory`, requestCustomerSubCategory)
      .pipe(map(response => {
        return response;
      }));
  }

  getCustomerProductSub2CategoryList(requestCustomerSubCategory: any) {

    return this.http.post<any>(`${this.url}/api/customer/getcustomerproductsubcategoryList`, requestCustomerSubCategory)
      .pipe(map(response => {
        return response;
      }));
  }



  getCustomerPriceCategories(customerId) {
    return this.http.get<any>(`${this.url}/api/customer/getcustomerpricecategory/${customerId}`);
  }
  getCustomerContactList(customerId) {
    return this.http.get<any>(`${this.url}/api/customer/getcustomercontactList/${customerId}`);
  }
  getCustomerAddressList(customerId) {
    return this.http.get<any>(`${this.url}/api/customer/getcustomerAddress/${customerId}`);
  }


  GetCustomerByCustomerId(id: number) {
    return this.http.get<any>(`${this.url}/api/customer/GetCustomerByCustomerId/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }

  getCustomerDataSourceList(model: CommonDataSourceRequest, term?: string) {
    console.log("***** getCustomerDataSourceList", model);
    model.searchText = "";
    if (term) {
      model.searchText = term;
      model.idList = null;
      model.id = null;
    }
    return this.http.post<any>(`${this.url}/api/Customer/GetCustomerDatasource`, model)
      .pipe(map(response => {
        console.log(" ***** response", response);
        return response.dataSourceList;
      }));
  }

  getPriceCategoryListByCustomerId(model: CommonCustomerSourceRequest, term?: string) {
    model.searchText = "";
    if (term) {
      model.searchText = term;
      model.idList = null;
    }
    return this.http.post<any>(`${this.url}/api/Customer/price-category-by-customer`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }

  getCustomerDataSource(model: CustomerDataSourceRequest, term?: string) {
    model.searchText = term ? term : model.searchText;
    return this.http.post<any>(`${this.url}/api/Customer/GetCustomerDataSourceList`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }

  getCustomerBuyerDataSource(model: BuyerDataSourceRequest, serviceId, term?: string) {
    model.searchText = term ? term : model.searchText;
    model.serviceId = serviceId;
    return this.http.post<any>(`${this.url}/api/CustomerBuyer/GetBuyerDataSourceList`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }

  getCustomerContactDataSource(model: CustomerContactSourceRequest, serviceId, term?: string) {
    model.searchText = term ? term : model.searchText;
    model.serviceId = serviceId;
    return this.http.post<any>(`${this.url}/api/CustomerContact/GetCustomerContactDataSourceList`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }
  getCustomerDataSourceBySupplier(model: CommonDataSourceRequest, term?: string) {
    model.searchText = term ? term : model.searchText;
    return this.http.post<any>(`${this.url}/api/Customer/get-customer-by-supplier`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }
  async getCustomerKAMDetails(customerId): Promise<CustomerKAMDetail> {
    return await this.http.get<CustomerKAMDetail>(`${this.url}/api/Customer/GetCustomerKAMDetails/${customerId}`).toPromise();
  }
  getCustomerProductCategories(customerId: number) {
    return this.http.get<any>(`${this.url}/api/customer/get-customer-product-category/${customerId}`)
      .pipe(map(response => {
        return response;
      }));
  }

  getcustomerpricedata(customerId: number) {
    return this.http.get<any>(`${this.url}/api/customer/getcustomerpricedata/${customerId}`)
      .pipe(map(response => {
        return response;
      }));
  }

  getCustomerSeasonConfig(customerId: number) {
    return this.http.get<any>(`${this.url}/api/customer/get-season-config/${customerId}`)
      .pipe(map(response => {
        return response;
      }));
  }


  getCustomerEntityList(customerId: number) {
    return this.http.get<any>(`${this.url}/api/customer/get-customer-entitylist/${customerId}`)
      .pipe(map(response => {
        return response;
      }));
  }


  getCustomerSisterCompany(customerId: number) {
    return this.http.get<any>(`${this.url}/api/customer/get-customer-sisterCompany/${customerId}`)
      .pipe(map(response => {
        return response;
      }));
  }

  getCustomerCommonDataSourceList(model: CustomerCommonDataSourceRequest, userTypeId: number, term?: string) {
    console.log("***** getCustomerDataSourceList", model);

    model.isStatisticsVisible = false;
    if (userTypeId == UserType.Supplier || userTypeId == UserType.Factory)
      model.isStatisticsVisible = true;

    model.searchText = "";
    if (term) {
      model.searchText = term;
      model.idList = null;
    }
    return this.http.post<any>(`${this.url}/api/Customer/GetCustomerDatasource`, model)
      .pipe(map(response => {
        console.log(" ***** response", response);
        return response.dataSourceList;
      }));
  }

  getCustomerListByUserType(model: CustomerCommonDataSourceRequest, userTypeId?: number, term?: string) {

    model.isStatisticsVisible = false;
    if (userTypeId && (userTypeId == UserType.Supplier || userTypeId == UserType.Factory))
      model.isStatisticsVisible = true;

    model.searchText = "";
    if (term) {
      model.searchText = term;
      model.idList = null;
    }
    return this.http.post<any>(`${this.url}/api/Customer/get-customer-by-userType`, model)
      .pipe(map(response => {        
        return response.dataSourceList;
      }));
  }
}


