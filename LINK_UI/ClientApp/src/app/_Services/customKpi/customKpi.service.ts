import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, first } from 'rxjs/operators';
import { CustomkpiModel } from '../../_Models/kpi/customkpimodel';
import { KpiTeamplateModel, KpiTeamplateRequest } from 'src/app/_Models/kpi/kpi-teamplate-model';

@Injectable({ providedIn: 'root' })

export class CustomKpiService {

  url : string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }

  getKpiSummary() {
    return this.http.get<any>(`${this.url}/api/KPICustom/GetSummary`)
      .pipe(map(response => {

        return response;
      }));
  }

  exportSummary(request: CustomkpiModel) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/KPICustom/ExportInspectionSearchSummary`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

  getKpiTeamplateSummary(request: KpiTeamplateRequest) {
    return this.http.post<KpiTeamplateModel>(`${this.url}/api/KPICustom/KpiTeamplateSummary`, request)
      .pipe(map(response => {
        return response;
      }));
  }
}
