import { Injectable, inject, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, first } from 'rxjs/operators';

import { Observable } from 'rxjs';
import { InspectionBookingsummarymodel } from 'src/app/_Models/booking/inspectionbookingsummarymodel';
import { UploadCustomReportRequest } from 'src/app/_Models/Report/reportmodel';

@Injectable({
  providedIn: 'root'
})
export class InternalFBReportService {

    url: string
    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
      this.url = baseUrl;
  
      if (this.url.charAt(this.url.length - 1) == '/')
        this.url = this.url.substring(0, this.url.length - 1);
    } 


    exportInternalFBReports(request: InspectionBookingsummarymodel) {
        const headers = new HttpHeaders({
          'Content-Type': 'application/json',
          'Accept': 'application/json'
        });
        return this.http.post<Blob>(`${this.url}/api/InternalFBReports/ExportInternalFBReports`, request, { headers: headers, responseType: 'blob' as 'json' });
      }
      //update the custom report
      updateCustomReport(model: UploadCustomReportRequest) {
        return this.http.post<any>(`${this.url}/api/Report/UpdateCustomReport`, model)
          .pipe(map(response => {
            return response;
          }));
      }

}
