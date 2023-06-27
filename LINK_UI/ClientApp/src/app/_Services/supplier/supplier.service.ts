import { ServiceType } from './../../_Models/quotation/quotation.model';
import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { EditSuplierModel } from '../../_Models/supplier/edit-supplier.model';
import { CommonDataSourceRequest, SupplierDataSourceRequest, CommonSupplierSourceRequest, DataSourceResponse, SupplierCommonDataSourceRequest, SupplierAllDataSourceRequest } from 'src/app/_Models/common/common.model';
import { mode } from 'crypto-js';
import { of } from 'rxjs';
import { FactoryDataSourceRequest } from 'src/app/_Models/statistics/defect-dashboard.model';
import { SupplierGradeRequest } from 'src/app/_Models/quotation/quotation.model';

@Injectable({ providedIn: 'root' })
export class SupplierService {

  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }

  getSupplierSummary() {

    return this.http.get<any>(`${this.url}/api/supplier`)
      .pipe(map(response => {
        return response;
      }));
  }

  getDataSummary(request) {
    return this.http.post<any>(`${this.url}/api/supplier/Search`, request)
      .pipe(map(response => {

        return response;
      }));
  }

  getChildDataSummary(id, type) {
    return this.http.get<any>(`${this.url}/api/supplier/GetSupplierChild/${id}/${type}`)
      .pipe(map(response => {
        return response;
      }));
  }

  deleteSupplier(id: number) {
    return this.http.get<any>(`${this.url}/api/supplier/delete/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }

  getEditSupplier(id) {

    if (id == null)
      return this.http.get<any>(`${this.url}/api/supplier/add`);
    else
      return this.http.get<any>(`${this.url}/api/supplier/edit/${id}`);
  }

  getStates(countryId) {
    return this.http.get<any>(`${this.url}/api/supplier/states/${countryId}`)
      .pipe(map(response => {
        return response;
      }));
  }

  getCities(stateId) {
    return this.http.get<any>(`${this.url}/api/supplier/cities/${stateId}`)
      .pipe(map(response => {
        return response;
      }));
  }

  saveSupplier(model: EditSuplierModel) {
    return this.http.post<any>(`${this.url}/api/supplier/save`, model)
      .pipe(map(response => {
        return response;
      }));
  }


  //get supplier list by customer id
  getSuppliersbyCustomer(customerId) {
    return this.http.get<any>(`${this.url}/api/supplier/GetsupplierBycustomerid/${customerId}`)
      .pipe(map(response => {
        return response;
      }));
  }

  getFactorybySupplier(supplierId) {
    return this.http.get<any>(`${this.url}/api/supplier/GetfactoryBysupid/${supplierId}`)
      .pipe(map(response => {
        return response;
      }));
  }
  //get factory list by supplier and customer
  GetfactoryBycustomeridsupId(cusid, supplierId) {
    return this.http.get<any>(`${this.url}/api/supplier/GetfactoryBycustomeridsupId/${cusid}/${supplierId}`)
      .pipe(map(response => {
        return response;
      }));
  }

  getCounties(cityId) {
    return this.http.get<any>(`${this.url}/api/location/cities/counties/${cityId}`)
      .pipe(map(response => {
        return response;
      }));
  }

  getTowns(countyId) {
    return this.http.get<any>(`${this.url}/api/location/county/town/${countyId}`)
      .pipe(map(response => {
        return response;
      }));
  }

  getSupplierByName(supplierName, typeValues: any) {
    var type;
    if (typeValues.length > 1 || typeValues.length == 0) {
      type = 0;
    }
    else {
      type = typeValues[0].id;
    }
    return this.http.get<any>(`${this.url}/api/supplier/GetSupplierByName/${supplierName}/${type}`)
      .pipe(map(response => {
        return response.data;
      }));
  }

  getFactoryDataSourceList(model: CommonDataSourceRequest, term?: string) {
    model.searchText = "";
    if (term) {
      model.searchText = term;
      model.idList = null;
    }
    return this.http.post<any>(`${this.url}/api/Supplier/getfactorydatasource`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }

  getSupplierCommonDataSourceList(model: SupplierCommonDataSourceRequest, term?: string) {
    model.searchText = "";
    if (term) {
      model.searchText = term;
      model.idList = null;
    }
    return this.http.post<any>(`${this.url}/api/Supplier/getfactorydatasource`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }

  getFactoryOrSupplierList(model: CommonSupplierSourceRequest, term?: string) {
    model.searchText = term ? term : model.searchText;
    return this.http.post<any>(`${this.url}/api/Supplier/getSupplierorFactoryList`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }

  getSupplierDataSource(model: SupplierDataSourceRequest, serviceId: number, term?: string) {
    model.searchText = "";
    if (term) {
      model.searchText = term;
      model.supplierIds = null;
    }
    model.serviceId = serviceId;
    return this.http.post<any>(`${this.url}/api/Supplier/GetSupplierDataSourceList`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }

  getSupplierListByName(item, term, supplierList) {
    if (supplierList && supplierList.length > 0 && !term) {
      const obsFrom3 = of(supplierList);
      return obsFrom3;
    }
    else {
      item.poDetails.supplierRequest.searchText = "";
      if (term)
        item.poDetails.supplierRequest.searchText = term;

      return this.http.post<any>(`${this.url}/api/Supplier/GetSupplierDataSourceList`, item.poDetails.supplierRequest)
        .pipe(
          map(

            response => {
              return response.dataSourceList;
            }));
    }

  }

  async getBaseSupplierDataSource(model: SupplierDataSourceRequest): Promise<DataSourceResponse> {
    return await this.http.post<DataSourceResponse>(`${this.url}/api/Supplier/GetSupplierDataSourceList`, model).toPromise();
  }

  getSupplierContactByBooking(bookingId, supType, serviceType) {
    return this.http.get<any>(`${this.url}/api/Supplier/GetSupplierContactByBooking/${bookingId}/${supType}/${serviceType}`).pipe(map(response => { return response; }));
  }

  getFactoryOrSupplierDataSource(model: FactoryDataSourceRequest, term?: string) {
    model.searchText = term ? term : model.searchText;
    return this.http.post<any>(`${this.url}/api/Supplier/supplier-factory-data`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }


  getSupplierByCountryDataSource(model: CommonSupplierSourceRequest, term?: string) {
    model.searchText = term ? term : model.searchText;
    return this.http.post<any>(`${this.url}/api/Supplier/GetSupplierByCountryDatasource`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  GetSupplierList(model: CommonDataSourceRequest, term?: string) {
    model.searchText = term ? term : model.searchText;
    return this.http.post<any>(`${this.url}/api/Supplier/GetSupplierList`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }

  GetFactoryList(model: CommonDataSourceRequest, term?: string) {
    model.searchText = term ? term : model.searchText;
    return this.http.post<any>(`${this.url}/api/Supplier/GetFactoryList`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }

  exportSummary(request) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/Supplier/export`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

  getSupplierFactoryAddress(id: number) {
    return this.http.get<any>(`${this.url}/api/Supplier/getsupplierFactorAddress/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }

  getBaseSupplierContactDetails(id: number) {
    return this.http.get<any>(`${this.url}/api/Supplier/get-base-supplier-contact-data/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }

  getExistingSupplierDetails(model: EditSuplierModel) {
    return this.http.post<any>(`${this.url}/api/Supplier/getExistSupplierDetails`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  addEntityIntoSupplier(data: any) {
    return this.http.post<any>(`${this.url}/api/supplier/addEntityIntoSupplier`, data)
      .pipe(map(response => {
        return response;
      }));
  }

  getSupplierLevelByCustomerId(customerId: number) {
    return this.http.get<any>(`${this.url}/api/supplier/getSupplierLevelByCustomerId/${customerId}`)
      .pipe(map(response => {
        return response;
      }));
  }

  async getSupplierGrade(request: SupplierGradeRequest) {
    return this.http.post<any>(`${this.url}/api/supplier/gradeBySupplierIdAndCustomerIdAndBookingIds`, request).toPromise();
  }

  async getAllSupplierList(model: SupplierAllDataSourceRequest): Promise<DataSourceResponse> {
    return await this.http.post<DataSourceResponse>(`${this.url}/api/Supplier/GetSupplierDataSourceList`, model).toPromise();
  }

}


