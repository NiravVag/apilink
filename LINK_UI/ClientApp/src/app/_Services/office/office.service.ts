import { Injectable, inject, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, first } from 'rxjs/operators';
import { EditOfficeModel } from '../../_Models/office/edit-officemodel';
import { CommonDataSourceRequest } from 'src/app/_Models/common/common.model';

@Injectable({
  providedIn: 'root'
})
export class OfficeService {

  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }
  getOfficeSummary() {
    return this.http.get<any>(`${this.url}/api/OfficeLocation`)
      .pipe(map(Response => { return Response; }));
  }
  getOfficeSearchSummary(request) {
    return this.http.post<any>(`${this.url}/api/OfficeLocation/GetSearchOffice`, request)
      .pipe(map(response => { return response; }));
  }
  getEditOffice(id) {
    if (id == null)
      id = "";
    return this.http.get<any>(`${this.url}/api/OfficeLocation/office/edit/${id}`);
  }
  saveOffice(model:EditOfficeModel) {
    return this.http.post<any>(`${this.url}/api/OfficeLocation/office/save`, model)
      .pipe(map(response => {
        return response;
      }));
  } 
  getOfficeforInternalUser() {
    return this.http.get<any>(`${this.url}/api/OfficeLocation/getofficeforinternal`)
    .pipe(map(response => {
      return response;
    }));
  }
  getOfficeList() {
    return this.http.get<any>(`${this.url}/api/OfficeLocation/getLocationList`)
      .pipe(map(Response => { return Response; }));
  }

  //get office list by login office access
  getOfficeListByOfficeAccess(model: CommonDataSourceRequest, term?: string) {
    model.searchText = term ? term : model.searchText;
    return this.http.post<any>(`${this.url}/api/OfficeLocation/get-office-by-office-access`,model)
    .pipe(map(response => {
      return response.dataSourceList;
    }));
  }
  getOfficeDetails() {
    return this.http.get<any>(`${this.url}/api/OfficeLocation/getofficelocationdetails`)
      .pipe(map(Response => { return Response; }));
  }

  getPositions()   {
    return this.http.get<any>(`${this.url}/api/humanresource/positions`)
      .pipe(map(Response => { return Response; }));
  }

  getStaffsByOffice(id : number) {
    return this.http.get<any>(`${this.url}/api/humanresource/${id}/staffs`)
      .pipe(map(Response => { return Response; }));
  }
}
