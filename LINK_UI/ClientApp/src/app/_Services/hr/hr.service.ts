import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, first } from 'rxjs/operators';
import { EditStaffModel, attachedFileModel, HRProfileResponse, HROutSourceCompanyRequest } from '../../_Models/hr/edit-staff.model';
import { HolidayRequest } from '../../_Models/hr/holidaymaster.model';
import { StaffSummaryModel } from '../../_Models/hr/staffsummary.model';
import { LeaveRequestModel } from '../../_Models/hr/leave-request.model';
import { LeaveSummaryModel } from '../../_Models/hr/leave-summary.model';
import { CommonDataSourceRequest, UserDataSourceRequest } from 'src/app/_Models/common/common.model';
import { CommonCountySourceRequest } from 'src/app/_Models/common/common.model';


@Injectable({ providedIn: 'root' })
export class HrService {

  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }

  getStaffSummary() {

    return this.http.get<any>(`${this.url}/api/HumanResource`)
      .pipe(map(response => {
        return response;
      }));
  }

  GetGender() {
    return this.http.get<any>(`${this.url}/api/HumanResource/GetUserGender`);
  }

  getDataSummary(request) {
    return this.http.post<any>(`${this.url}/api/HumanResource/Search`, request)
      .pipe(map(response => {

        return response;
      }));
  }

  getEditStaff(id) {

    if (id == null)
      id = "";

    return this.http.get<any>(`${this.url}/api/HumanResource/staff/edit/${id}`);
  }

  saveStaff(model: EditStaffModel) {
    return this.http.post<any>(`${this.url}/api/HumanResource/staff/save`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  uploadPicture(staffId, file) {

    const formData = new FormData();

    formData.append(file.name, file);

    //const uploadReq = new HttpRequest('POST', `api/upload`, formData, {
    //  reportProgress: true,
    //});

    return this.http.post(`${this.url}/api/HumanResource/staff/uphoto/${staffId}`, formData)
      .pipe(map(response => {
        return response;
      }));
  }

  uploadAttachedFiles(staffid, attachedList: Array<attachedFileModel>) {

    const formData = new FormData();

    attachedList.map(x => {
      formData.append(x.fileName, x.file);
    });

    return this.http.post(`${this.url}/api/HumanResource/staff/attached/${staffid}`, formData)
      .pipe(map(response => {
        return response;
      }));

  }

  getPicture(staffId) {
    return this.http.get<any>(`${this.url}/api/HumanResource/staff/photo/${staffId}`)
      .pipe(map(response => {
        return response;
      }));
  }

  getFile(id) {
    return this.http.get(`${this.url}/api/HumanResource/staff/file/${id}`, { responseType: 'blob' });
  }

  deleteStaff(request) {
    return this.http.post<any>(`${this.url}/api/HumanResource/staff/delete`, request)
      .pipe(map(response => {
        return response;
      }));
  }

  getSubDepratments(idDept) {
    return this.http.get<any>(`${this.url}/api/HumanResource/staff/subdepts/${idDept}`)
      .pipe(map(response => {
        return response;
      }));
  }

  getStates(countryId) {
    return this.http.get<any>(`${this.url}/api/HumanResource/staff/states/${countryId}`)
      .pipe(map(response => {
        return response;
      }));
  }

  getCities(stateId) {
    return this.http.get<any>(`${this.url}/api/HumanResource/staff/cities/${stateId}`)
      .pipe(map(response => {
        return response;
      }));
  }

  getHolidaySummary() {
    return this.http.get<any>(`${this.url}/api/HumanResource/holiday-master`)
      .pipe(map(response => {
        return response;
      }));
  }

  getHolidaysData(request) {
    return this.http.post<any>(`${this.url}/api/HumanResource/holidays/search`, request)
      .pipe(map(response => {

        return response;
      }));
  }

  updateHoliday(request: HolidayRequest) {
    return this.http.post<any>(`${this.url}/api/HumanResource/holiday/edit`, request)
      .pipe(map(response => {
        return response;
      }));
  }

  deleteHoliday(id: number, forAllIterations: boolean) {
    return this.http.post<any>(`${this.url}/api/HumanResource/holiday/delete`, { id: id, forAllIterations: forAllIterations })
      .pipe(map(response => {
        return response;
      }));
  }

  getOfficeControls() {
    return this.http.get<any>(`${this.url}/api/HumanResource/OfficesControl`)
      .pipe(map(response => {
        return response;
      }));
  }

  getOffices(id: any) {
    return this.http.get<any>(`${this.url}/api/HumanResource/OfficesControl/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }

  saveOfficeControl(id: any, data: any) {
    return this.http.post<any>(`${this.url}/api/HumanResource/OfficesControl/save`, { staffId: id, data: data })
      .pipe(map(response => {
        return response;
      }));
  }


  exportStaff(request: StaffSummaryModel) {

    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });

    return this.http.post<Blob>(`${this.url}/api/HumanResource/export`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

  getLeaveRequest(id) {
    if (id == null)
      id = "";

    return this.http.get<any>(`${this.url}/api/HumanResource/leave-request/${id}`);
  }

  saveLeaveRequest(model: LeaveRequestModel) {
    return this.http.post<any>(`${this.url}/api/HumanResource/leave/save`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  getLeaveSummary() {
    return this.http.get<any>(`${this.url}/api/HumanResource/leave-summary/`);
  }

  getLeaveDataSummary(model: LeaveSummaryModel) {
    return this.http.post<any>(`${this.url}/api/HumanResource/leave-summary`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  exportLeaveSummary(model: LeaveSummaryModel) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });

    return this.http.post<Blob>(`${this.url}/api/HumanResource/leave/export`, model, { headers: headers, responseType: 'blob' as 'json' });

  }

  setStatus(id: number, idStatus: number) {
    return this.http.get<any>(`${this.url}/api/HumanResource/leave/status/${id}/${idStatus}`)
      .pipe(map(response => {
        return response;
      }));
  }

  reject(id: number, comment: string) {
    return this.http.post<any>(`${this.url}/api/HumanResource/leave/reject/${id}`, { comment: comment })
      .pipe(map(response => {
        return response;
      }));
  }

  getNumberDays(startDate: any, endDate: any, startDayType: number, endDayType: number) {
    let request: any = { startDate: startDate, endDate: endDate, startDayType: startDayType, endDayType: endDayType };
    return this.http.post<any>(`${this.url}/api/HumanResource/leave/days`, request)
      .pipe(map(response => {
        return response;
      }));
  }

  async getProfileList() {
    return await this.http.get<HRProfileResponse>(`${this.url}/api/HumanResource/profileList`).toPromise();
  }

  //get staff list who has inspector role(QC)
  getQCDataSource(model: CommonDataSourceRequest, term?: string) {
    model.searchText = term ? term : "";
    return this.http.post<any>(`${this.url}/api/HumanResource/get-staff-details`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }
  //Get County By City and search text
  getCountyByCityDataSourceList(model: CommonCountySourceRequest, term?: string) {
    model.searchText = term ? term : model.searchText;
    return this.http.post<any>(`${this.url}/api/Location/GetCountyByCityDatasource`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }

  getStatusList() {
    return this.http.get<any>(`${this.url}/api/HumanResource/get-status-list`);
  }

  getHukoLocationDataSourceList(model: CommonDataSourceRequest, term?: string) {
    model.searchText = "";
    if (term) {
      model.searchText = term;
      model.idList = null;
    }
    return this.http.post<any>(`${this.url}/api/HumanResource/get-huko-location-list`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }

  getBandList() {
    return this.http.get<any>(`${this.url}/api/HumanResource/get-band-list`);
  }

  getSocialInsuranceTypeList() {
    return this.http.get<any>(`${this.url}/api/HumanResource/get-social-insurance-type-list`);
  }

  getStaffUserList(model: UserDataSourceRequest, term?: string) {
    model.searchText = term ? term : "";
    return this.http.post<any>(`${this.url}/api/humanResource/get-staff-summary`, model)
      .pipe(map(response => {
        return response.dataSourceList;
      }));
  }

  getHROutSourceCompanyDataList() {
    return this.http.get<any>(`${this.url}/api/HumanResource/get-all-hr-outsource-company-list`);
  }

  getHROutSourceCompanyList(model: HROutSourceCompanyRequest, term?: string) {
    model.searchText = "";
    if (term) {
      model.searchText = term;
      model.id = null;
    }
    return this.http.post<any>(`${this.url}/api/humanResource/get-hr-outsource-company-list`, model)
      .pipe(map(response => {
        return response.hrOutSourceCompanyList;
      }));
  }

  saveHROutSourceCompany(request) {
    return this.http.post<any>(`${this.url}/api/HumanResource/save-hr-outsource-company`, request)
      .pipe(map(response => {
        return response;
      }));
  }

  getStaffListByCompanyIds(companyIds) {
    return this.http.post<any>(`${this.url}/api/humanResource/get-stafflist-by-company`, companyIds)
      .pipe(map(response => {
        return response;
      }));
  }

  getHRPayrollCompanyList() {
    return this.http.get<any>(`${this.url}/api/humanResource/get-hr-payroll-company-list`);
  }


}
