import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { EditLabModel } from '../../_Models/lab/edit-lab.model';

@Injectable({ providedIn: 'root' })
export class LabService {
  url : string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;
    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }
  saveLab(model: EditLabModel) {
    return this.http.post<any>(`${this.url}/api/lab/save`, model)
      .pipe(map(response => {
        return response;
      }));
  }
  updateLab(model: EditLabModel) {
    return this.http.put<any>(`${this.url}/api/lab/save`, model)
      .pipe(map(response => {
        return response;
      }));
  }
  getMainType() {
    return this.http.get<any>(`${this.url}/api/lab/type`)
      .pipe(map(response => {
        return response;
      }));
  }
  getAddressType() {
    return this.http.get<any>(`${this.url}/api/lab/address/type`)
      .pipe(map(response => {
        return response;
      }));
  }
  getCustomer() {
    return this.http.get<any>(`${this.url}/api/customer`)
      .pipe(map(response => {
        return response;
      }));
  }
  getCountry() {
    return this.http.get<any>(`${this.url}/api/location`)
      .pipe(map(response => {
        return response;
      }));
  }
  getStates(countryId) {
    return this.http.get<any>(`${this.url}/api/location/states/${countryId}`)
      .pipe(map(response => {
        return response;
      }));
  }
  getCities(stateId) {
    return this.http.get<any>(`${this.url}/api/location/cities/${stateId}`)
      .pipe(map(response => {
        return response;
      }));
  }
  getDataSearch(request) {
    return this.http.post<any>(`${this.url}/api/lab`, request)
      .pipe(map(response => {
        return response;
      }));
  }
  deleteLab(id:number) {
    return this.http.delete<any>(`${this.url}/api/lab/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }
  getEditLab(id:number) {
      return this.http.get<any>(`${this.url}/api/lab/${id}`); //edit
  }
  getLabName() {
    return this.http.get<any>(`${this.url}/api/lab`)
      .pipe(map(response => {
        return response;
      }));
  }
}
