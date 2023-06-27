import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { DMModuleListResponse } from '../../_Models/data-management/dm.module.model';
import { InternalBusinessModel, KpiTemplateViewRequest } from '../../_Models/kpi/template.view.model';
import { DeleteTemplateResponse } from '../../_Models/kpi/template.model';
import { DataManagementItem, DataManagementItemResponse, DataManagementListRequest, DataManagementListResponse, DataManagementRightRequest, DataManagementRightResponse, DMUserManagementDataEditResponse, SaveDataManagementItemResponse, SaveDataManagementRightRequest } from '../../_Models/data-management/dm.model';
import { RolesResponse } from '../../_Models/user/role.model';
import { TreeviewItem } from 'ngx-treeview';
import { map } from 'rxjs/operators';
import { DMUserSummaryModel } from 'src/app/_Models/data-management/dmusermanagementsummary.model';


@Injectable({ providedIn: 'root' })
export class DataManagementService {
  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }

  async getMainModuleList(): Promise<DMModuleListResponse> {
    return await this.http.get<DMModuleListResponse>(`${this.url}/api/data-management/modules`).toPromise();
  }


  async getDataManagementItem(id: number): Promise<DataManagementItemResponse> {
    return await this.http.get<DataManagementItemResponse>(`${this.url}/api/data-management/${id}`).toPromise();
  }

  getModuleList() {
    return this.http.get<any>(`${this.url}/api/data-management/get-module-list`);
  }

  async getModuleDataList() {
    return await this.http.get<any>(`${this.url}/api/data-management/get-module-list`).toPromise();
  }

  async search(request: DataManagementListRequest): Promise<DataManagementListResponse> {
    return await this.http.post<DataManagementListResponse>(`${this.url}/api/data-management/search`, request).toPromise();
  }

  getDMDataSummary(request) {
    return this.http.post<any>(`${this.url}/api/data-management/search`, request)
      .pipe(map(response => {

        return response;
      }));
  }

  export(request: KpiTemplateViewRequest) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/kpi/template/export`, request, { headers: headers, responseType: 'blob' as 'json' });
  }


  exportInternal(model: InternalBusinessModel) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/kpicustom/ppt`, model, { headers: headers, responseType: 'blob' as 'json' });
  }

  async delete(id: number): Promise<DeleteTemplateResponse> {
    return await this.http.delete<DeleteTemplateResponse>(`${this.url}/api/kpi/template/${id}/delete`).toPromise();
  }

  async save(model: DataManagementItem): Promise<SaveDataManagementItemResponse> {
    return await this.http.post<SaveDataManagementItemResponse>(`${this.url}/api/data-management/save`, model).toPromise();
  }

  async getRights(request: DataManagementRightRequest): Promise<DataManagementRightResponse> {
    return await this.http.post<DataManagementRightResponse>(`${this.url}/api/data-management/rights`, request).toPromise();
  }

  async saveRights(request: SaveDataManagementRightRequest): Promise<DataManagementRightResponse> {
    if (request && request.id > 0) {
      return await this.http.put<DataManagementRightResponse>(`${this.url}/api/data-management/update-rights/${request.id}`, request).toPromise();
    }
    else {
      return await this.http.post<DataManagementRightResponse>(`${this.url}/api/data-management/save-rights`, request).toPromise();
    }
  }

  async getRoles(): Promise<RolesResponse> {
    return await this.http.get<RolesResponse>(`${this.url}/api/useraccount/roles`).toPromise();
  }

  getBooks(): TreeviewItem[] {
    const childrenCategory = new TreeviewItem({
      text: 'Children', value: 1, collapsed: true, children: [
        { text: 'Baby 3-5', value: 11 },
        { text: 'Baby 6-8', value: 12 },
        { text: 'Baby 9-12', value: 13 }
      ]
    });
    const itCategory = new TreeviewItem({
      text: 'IT', value: 9, children: [
        {
          text: 'Programming', value: 91, children: [{
            text: 'Frontend', value: 911, children: [
              { text: 'Angular 1', value: 9111 },
              { text: 'Angular 2', value: 9112 },
              { text: 'ReactJS', value: 9113, disabled: true }
            ]
          }, {
            text: 'Backend', value: 912, children: [
              { text: 'C#', value: 9121 },
              { text: 'Java', value: 9122 },
              { text: 'Python', value: 9123, checked: false, disabled: true }
            ]
          }]
        },
        {
          text: 'Networking', value: 92, children: [
            { text: 'Internet', value: 921 },
            { text: 'Security', value: 922 }
          ]
        }
      ]
    });
    const teenCategory = new TreeviewItem({
      text: 'Teen', value: 2, collapsed: true, disabled: true, children: [
        { text: 'Adventure', value: 21 },
        { text: 'Science', value: 22 }
      ]
    });
    const othersCategory = new TreeviewItem({ text: 'Others', value: 3, checked: false, disabled: true });
    return [childrenCategory, itCategory, teenCategory, othersCategory];
  }

  deleteDMData(id: number) {
    return this.http.get<any>(`${this.url}/api/data-management/delete/${id}`)
      .pipe(map(response => {
        return response;
      }));
  }


  getUserRights(model: DMUserSummaryModel) {
    return this.http.post<any>(`${this.url}/api/data-management/getUserRightSummary`, model);
  }

  async getEditDmUserManagement(id: number): Promise<DMUserManagementDataEditResponse> {
    return await this.http.get<DMUserManagementDataEditResponse>(`${this.url}/api/data-management/getEditDMUserManagement/${id}`).toPromise();
  }


  deleteDmUserManagement(id: number) {
    return this.http.delete<DMUserManagementDataEditResponse>(`${this.url}/api/data-management/deleteDMUserManagement/${id}`);
  }

  isUploadRight(moduleId) {
    return this.http.get<boolean>(`${this.url}/api/data-management/checkDmUploadRight/${moduleId}`);
  }

  getDmModulesById(id) {
    return this.http.get<any>(`${this.url}/api/data-management/getModulesByDmRoleId/${id}`);
  }
}
