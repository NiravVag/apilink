import { InvoiceReportTemplateRequest } from './../../_Models/common/common.model';
import { Injectable, Inject } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { promise } from "protractor";
import { map } from "rxjs/operators";
import { InvoicePdfAvailableRequest, InvoicePdfCreatedResponse, InvoiceSummaryRequestModel } from "src/app/_Models/invoice/invoicesummary.model";
import { InvoiceKpiTemplateRequest } from 'src/app/_Models/invoice/invoice-kpi-template-request';

@Injectable({
    providedIn: 'root'
})
export class InvoiceSummaryService {

    url: string
    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.url = baseUrl;

        if (this.url.charAt(this.url.length - 1) == '/')
            this.url = this.url.substring(0, this.url.length - 1);
    }

    search(model: InvoiceSummaryRequestModel) {
        return this.http.post<any>(`${this.url}/api/invoice/getInvoiceSummary`, model);
    }

    checkPdfInvoice(model: InvoicePdfAvailableRequest) {
        return this.http.post<InvoicePdfCreatedResponse>(`${this.url}/api/invoice/CheckInvoicePdfCreated`, model);
    }

    getInvoiceBooking(invoiceNo, serviceId) {
        return this.http.get<any>(`${this.url}/api/invoice/getInvoiceBookingSummary/${invoiceNo}/${serviceId}`);
    }

    getInvoiceReportTemplates(request: InvoiceReportTemplateRequest) {
        return this.http.post<any>(`${this.url}/api/invoice/getInvoiceReportTemplates`, request);
    }

    getInvoiceReportTemplateUrl() {
        return this.http.get<any>(`${this.url}/api/invoice/GetInvoiceReportTemplateUrl`);
    }

    cancelInvoice(invoiceNo) {
        return this.http.get<any>(`${this.url}/api/invoice/CancelInvoice/${invoiceNo}`);
    }


    exportSummary(request: InvoiceSummaryRequestModel) {
        const headers = new HttpHeaders({
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        });
        return this.http.post<Blob>(`${this.url}/api/invoice/ExportInvoiceSearchSummary`, request, { headers: headers, responseType: 'blob' as 'json' });
    }

    getInvoiceStatus() {
        return this.http.get<any>(`${this.url}/api/invoice/getInvoiceStatusList`);
    }

    getKpiTemplateList(request: InvoiceKpiTemplateRequest) {
        return this.http.post<any>(`${this.url}/api/invoice/getKpiTemplateList`, request);
    }

}
