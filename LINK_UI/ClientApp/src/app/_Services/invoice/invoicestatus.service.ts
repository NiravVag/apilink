import { Injectable, Inject } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { promise } from "protractor";
import { map } from "rxjs/operators";
import { InvoiceCommunicationSaveRequest, InvoiceCommunicationSaveResponse, InvoiceCommunicationTableResponse, InvoiceStatusRequestModel } from "src/app/_Models/invoice/invoicestatus.model";

@Injectable({
    providedIn: 'root'
})
export class InvoiceStatusService {

    url: string
    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.url = baseUrl;

        if (this.url.charAt(this.url.length - 1) == '/')
            this.url = this.url.substring(0, this.url.length - 1);
    }

    search(model: InvoiceStatusRequestModel) {
        return this.http.post<any>(`${this.url}/api/InvoiceStatus/getInvoiceStatusSummary`, model);
    }
      
  exportSummary(request: InvoiceStatusRequestModel) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/InvoiceStatus/ExportInvoiceSearchSummary`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

getStatusbyService(serviceId) {
    return this.http.get<any>(`${this.url}/api/InvoiceStatus/getStatusByService/${serviceId}`);
  }

  async communicationSave(saveModelRequest: InvoiceCommunicationSaveRequest): Promise<InvoiceCommunicationSaveResponse> {
    return await this.http.post<InvoiceCommunicationSaveResponse>(`${this.url}/api/InvoiceStatus/invoice-communication-save`, saveModelRequest).toPromise();
  }

  async communicationSummary(invoiceNo: string) {
    return this.http.get<InvoiceCommunicationTableResponse>(`${this.url}/api/InvoiceStatus/get-invoice-communication-summary/${invoiceNo}`).toPromise();
  }
}
