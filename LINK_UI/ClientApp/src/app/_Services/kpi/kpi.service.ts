import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ModuleListResponse } from '../../_Models/kpi/module.model';
import { KpiFilterListResponse } from '../../_Models/kpi/filter.model';
import { KpiColumnListResponse } from '../../_Models/kpi/column.model';
import { KpiTemplateListResponse, KpiTemplateItemResponse, TemplateModel, KpiSavetemplateResponse,  KpiTemplateListRequest, DeleteTemplateResponse } from '../../_Models/kpi/template.model';
import { KpiTemplateColumnListResponse } from '../../_Models/kpi/template.column.model';
import { KpiTemplateFilterListResponse } from '../../_Models/kpi/template.filter.model';
import { KpiDataSourceResponse, ViewDataResponse } from '../../_Models/kpi/datasource.model';
import { KpiTemplateViewResponse, KpiTemplateViewRequest, InternalBusinessModel } from '../../_Models/kpi/template.view.model';


@Injectable({ providedIn: 'root' })
export class KpiService
{
  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }

  async getModuleList(): Promise<ModuleListResponse> {
    return await this.http.get<ModuleListResponse>(`${this.url}/api/kpi/modules`).toPromise();
  }

  async GetSubModuleList(idModule : number): Promise<ModuleListResponse> {
    return await this.http.get<ModuleListResponse>(`${this.url}/api/kpi/modules/${idModule}/submodules`).toPromise();
  }

  async GetColumnList(idSubModule: number): Promise<KpiColumnListResponse> {
    return await this.http.get<KpiColumnListResponse>(`${this.url}/api/kpi/submodule/${idSubModule}/columns`).toPromise();
  }

  async GetFilterList(idSubModule: number): Promise<KpiFilterListResponse> {
    return await this.http.get<KpiFilterListResponse>(`${this.url}/api/kpi/submodule/${idSubModule}/filters`).toPromise();
  }

  async GetColumnListByModule(idModule: number): Promise<KpiColumnListResponse> {
    return await this.http.get<KpiColumnListResponse>(`${this.url}/api/kpi/module/${idModule}/columns`).toPromise();
  }

  async GetFilterListByModule(idModule: number): Promise<KpiFilterListResponse> {
    return await this.http.get<KpiFilterListResponse>(`${this.url}/api/kpi/module/${idModule}/filters`).toPromise();
  }

  async GetTemplateListByIdSubModule(idSubModule: number): Promise<KpiTemplateListResponse> {
    return await this.http.get<KpiTemplateListResponse>(`${this.url}/api/kpi/submodule/${idSubModule}/templates`).toPromise();
  }

  async GetTemplateList(): Promise<KpiTemplateListResponse> {
    return await this.http.get<KpiTemplateListResponse>(`${this.url}/api/kpi/templates`).toPromise();
  }

  async GetTemplate(id: number): Promise<KpiTemplateItemResponse> {
    return await this.http.get<KpiTemplateItemResponse>(`${this.url}/api/kpi/template/${id}`).toPromise();
  }
  async GetTemplateColumnList(idTemplate: number): Promise<KpiTemplateColumnListResponse> {
    return await this.http.get<KpiTemplateColumnListResponse>(`${this.url}/api/kpi/template/${idTemplate}/columns`).toPromise();
  }


  async GetTemplateFilterList(idTemplate: number): Promise<KpiTemplateFilterListResponse> {
    return await this.http.get<KpiTemplateFilterListResponse>(`${this.url}/api/kpi/template/${idTemplate}/filters`).toPromise();
  }

  async GetDataSource(idFilter: number): Promise<KpiDataSourceResponse> {
    return await this.http.get<KpiDataSourceResponse>(`${this.url}/api/kpi/filter/${idFilter}/data-source`).toPromise();
  }

  getLazyDataSource(term:string, idFilter : number, fieldName : string) {
    return this.http.get<Array<any>>(`${this.url}/api/kpi/filter/${idFilter}/data-lazy/${fieldName}/${term}`);
  }

  async saveTemplate(model: TemplateModel): Promise<KpiSavetemplateResponse> {
    return await this.http.post<KpiSavetemplateResponse>(`${this.url}/api/kpi/template/save`, model).toPromise();
  }

  async searchTemplates(request: KpiTemplateListRequest): Promise<KpiTemplateListResponse> {
    return await this.http.post<KpiTemplateListResponse>(`${this.url}/api/kpi/templates`, request).toPromise();
  }

  async GetViewTemplate(idTemplate: number): Promise<KpiTemplateViewResponse> {
    return await this.http.get<KpiTemplateViewResponse>(`${this.url}/api/kpi/template/view/${idTemplate}`).toPromise();
  }

  async ViewResult(request: KpiTemplateViewRequest): Promise<ViewDataResponse> {
    return await this.http.post<ViewDataResponse>(`${this.url}/api/kpi/template/view`, request).toPromise();
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
}
