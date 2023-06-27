import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, first } from 'rxjs/operators';
import { EditDfCustomerConfiguration } from 'src/app/_Models/dynamicfields/editdfcustomerconfiguration.model';
import { DFCustomerConfigsummarymodel, DfCustomerConfigurationRequest } from '../../_Models/dynamicfields/dfcustomerconfigsummary.model'

@Injectable({ providedIn: 'root' })
export class DynamicFieldService {

  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }

  getModules() {
    return this.http.get<any>(`${this.url}/api/dynamicField/getmodules`)
      .pipe(map(response => {
        return response;
      }));
  }

  getControlTypes() {
    return this.http.get<any>(`${this.url}/api/dynamicField/getcontroltypes`)
      .pipe(map(response => {
        return response;
      }));
  }
  getddlsourcetypes(customerId) {
    return this.http.get<any>(`${this.url}/api/dynamicField/getddlsourcetypelist/${customerId}`)
      .pipe(map(response => {
        return response;
      }));
  }
  getdfcontrolattributes(controlTypeId) {
    return this.http.get<any>(`${this.url}/api/dynamicField/getcontroltypeattributes/${controlTypeId}`)
      .pipe(map(response => {
        return response;
      }));
  }
  getddlsourcelist(typeId) {
    return this.http.get<any>(`${this.url}/api/dynamicField/getddlsourcelist/${typeId}`)
      .pipe(map(response => {
        return response;
      }));
  }
  saveDFCustomerConfiguration(model: EditDfCustomerConfiguration) {
    return this.http.post<any>(`${this.url}/api/dynamicfield/save`, model)
      .pipe(map(response => {
        return response;
      }));
  }
  getcustomerconfigurationlist(customerId, moduleId) {
    return this.http.get<any>(`${this.url}/api/dynamicField/getdfconfiguration/${customerId}/${moduleId}`)
      .pipe(map(response => {
        return response;
      }));
  }
  getparentddlsourcelist(customerId) {
    return this.http.get<any>(`${this.url}/api/dynamicField/getparentdropdowntypes/${customerId}`)
      .pipe(map(response => {
        return response;
      }));
  }
  searchDFCustomerConfigSummary(model: DFCustomerConfigsummarymodel) {
    return this.http.post<any>(`${this.url}/api/dynamicField/searchdfCustomerConfigSummary`, model)
      .pipe(map(response => {
        return response;
      }));
  }
  getDFCustomerConfiguration(id) {
    return this.http.get<any>(`${this.url}/api/dynamicField/getdfcustomerconfiguration/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }

  deleteDFCustomerConfiguration(id:number) {
    return this.http.delete<any>(`${this.url}/api/dynamicField/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }

  checkdfcustomerconfiginbooking(id) {
    return this.http.get<any>(`${this.url}/api/dynamicField/checkdfcustomerconfiginbooking/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }

  getgapcustomerconfigurationlist(request: DfCustomerConfigurationRequest) {
    return this.http.post<any>(`${this.url}/api/DynamicField/dfGapCuConfiguration`,request)
      .pipe(map(response => {
        return response;
      }));
  }
}