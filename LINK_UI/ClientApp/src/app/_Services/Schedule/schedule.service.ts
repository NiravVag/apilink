import { Injectable, inject, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, first } from 'rxjs/operators';
import { BookingItemSchedule, ScheduleModel, MandayModel, QcVisibilityBookingModel, bookingDataQcVisibleRequest } from "../../_Models/Schedule/schedulemodel";
import { scheduleAllocationModel, SaveScheduleModel, StaffSearchDataSource } from "../../_Models/Schedule/scheduleallocationmodel";
import { QcDataSourceRequest, CommonZoneSourceRequest } from '../../_Models/common/common.model';

@Injectable({
  providedIn: 'root'
})

export class ScheduleService {


  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }

  SearchInspectionBookingSummary(model: ScheduleModel) {
    return this.http.post<any>(`${this.url}/api/Schedule/ScheduleSummary`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  getCSStaffList() {
    return this.http.get<any>(`${this.url}/api/Schedule/GetCSStaffList`)
      .pipe(map(response => {
        return response;
      }));
  }

  saveStaffList(model: SaveScheduleModel) {
    return this.http.post<any>(`${this.url}/api/Schedule/SaveSchedule`, model)
      .pipe(map(response => {
        return response;
      }));
  }


  async getDuplicateTravelAllowance(model: SaveScheduleModel) {
    return this.http.post<any>(`${this.url}/api/Schedule/GetDuplicateTravelExpenseData`, model).toPromise();
  }


  getQCStaffList() {
    return this.http.get<any>(`${this.url}/api/Schedule/GetQCStaffList`)
      .pipe(map(response => {
        return response;
      }));
  }
  getBookingDetailsbyId(id: number) {
    return this.http.get<any>(`${this.url}/api/Schedule/GetBookingAllocation/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }
  exportSummary(request: ScheduleModel) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/Schedule/ExportScheduleSearchSummary`, request, { headers: headers, responseType: 'blob' as 'json' });
  }
  exportMandayForecast(request: ScheduleModel) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/Schedule/ExportMandayForecast`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

  //get stafflist on click
  getStaffList(request: StaffSearchDataSource, term?: string) {
    request.searchText = term ? term : "";
    return this.http.post<any>(`${this.url}/api/Schedule/GetQCDetails`, request)
      .pipe(map(response => {
        return response;
      }));
  }

  sendEmail(bookingIds) {
    return this.http.post<any>(`${this.url}/api/EmailSchedule/getScheduleQCEmail`, bookingIds)
      .pipe(map(response => {
        return response;
      }));
  }

  getMandayForecast(model: ScheduleModel) {
    return this.http.post<any>(`${this.url}/api/Schedule/GetMandayForecast`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  getLeaveDetails(date, location, zoneid) {
    return this.http.get<any>(`${this.url}/api/Schedule/GetStaffwithLeave/${date}/${location}/${zoneid}`)
      .pipe(map(response => {
        return response;
      }));
  }

  getQuotationManday(bookingId) {
    return this.http.get<any>(`${this.url}/api/Schedule/GetManday/${bookingId}`)
      .pipe(map(response => {
        return response;
      }))
  }

  saveManday(model: MandayModel) {
    return this.http.post<any>(`${this.url}/api/Schedule/SaveManday`, model)
      .pipe(map(response => {
        return response;
      }));
  }
  saveQcVisibility(model: bookingDataQcVisibleRequest) {
    return this.http.post<any>(`${this.url}/api/Schedule/UpdateQcVisibility`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  getQcDataSourceList(model: QcDataSourceRequest, term?: string) {
    model.searchText = term ? term : "";
    return this.http.post<any>(`${this.url}/api/Schedule/getqcdatasource`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }
  //Get zone by search text
  getZoneDataSourceList(model: CommonZoneSourceRequest, term?: string) {
    model.searchText = term ? term : model.searchText;
    return this.http.post<any>(`${this.url}/api/Location/GetZoneDatasource`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }
  getOfficeZoneDataSourceList(model: CommonZoneSourceRequest, term?: string) {
    model.searchText = term ? term : model.searchText;
    return this.http.post<any>(`${this.url}/api/Location/GetOfficeZoneDatasource`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }

  getQcVisibilityByBooking(model: QcVisibilityBookingModel) {
    return this.http.post<any>(`${this.url}/api/Schedule/GetQcVisibilityByBooking`, model)
      .pipe(map(response => {
        return response;
      }));
  }
  getproductdetails(bookingId: number) {
    return this.http.get<any>(`${this.url}/api/Schedule/getproductdetails/${bookingId}`)
      .pipe(map(response => {
        return response;
      }));
  }
}
