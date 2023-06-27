import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { SaleInvoiceSummaryRequestModel } from 'src/app/_Models/invoice/sale-invoice-summary-model';

@Injectable({
  providedIn: 'root'
})
export class SaleInvoiceSummaryService {

  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }

  search(model: SaleInvoiceSummaryRequestModel) {
    return this.http.post<any>(`${this.url}/api/saleinvoice/getSaleInvoiceSummary`, model);
  }
  
  exportSummary(request: SaleInvoiceSummaryRequestModel) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/saleinvoice/ExportSaleInvoiceSearchSummary`, request, { headers: headers, responseType: 'blob' as 'json' });
  }
}
