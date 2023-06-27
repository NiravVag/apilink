import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { ExtraFeeSummaryRequestModel } from "../../_Models/invoice/extrafeesummary.model";
@Injectable({
  providedIn: 'root'
})
export class ExtrafeesummaryService {
  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }
  getExtraFeeStatus() {
    return this.http.get<any>(`${this.url}/api/ExtraFees/getextrafeestatus`);
  }
  search(model: ExtraFeeSummaryRequestModel) {
    return this.http.post<any>(`${this.url}/api/ExtraFees/getExtrafeesSummary`, model);
  }
  exportSummary(request: ExtraFeeSummaryRequestModel) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/ExtraFees/ExportExtrafeesSearchSummary`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

}
