import { Inject, Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { EditManualInvoiceModel } from "src/app/_Models/invoice/edit-manual-invoice.model";
import { InvoiceMoExistsResult } from "src/app/_Models/invoice/editinvoice.model";
import { ManualInvoiceSummaryModel } from "src/app/_Models/invoice/manual-invoice-summary.model";

@Injectable({
  providedIn: "root",
})
export class ManualInvoiceService {
  url: string;
  constructor(private http: HttpClient, @Inject("BASE_URL") baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == "/")
      this.url = this.url.substring(0, this.url.length - 1);
  }

  saveManualInvoice(model: EditManualInvoiceModel) {
    if (model && model.id > 0) {
      return this.http.post<any>(
        `${this.url}/api/manualInvoice/update-manual-invoice`,
        model
      );
    } else {
      return this.http.post<any>(
        `${this.url}/api/manualInvoice/save-manual-invoice`,
        model
      );
    }
  }

  getManualInvoiceSummary(model: ManualInvoiceSummaryModel) {
    return this.http.post<any>(
      `${this.url}/api/manualInvoice/manual-invoice-summary`,
      model
    );
  }
  getManualInvoice(id: number):Promise<any> {
    return this.http.get<any>(
      `${this.url}/api/manualInvoice/get-manual-invoice/${id}`
    ).toPromise();
  }

  deleteManualInvoice(id: number) {
    return this.http.post<any>(
      `${this.url}/api/manualInvoice/delete-manual-invoice/${id}`,
      null
    );
  }
  async checkInvoiceNumberExist(
    invoiceNo: string
  ): Promise<boolean> {
    return this.http
      .get<any>(
        `${this.url}/api/manualInvoice/checkinvoicenumberexist/${invoiceNo}`
      )
      .toPromise();
  }

  exportManulInvoiceSummary(request: ManualInvoiceSummaryModel) {
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });
    return this.http.post<Blob>(`${this.url}/api/manualInvoice/export-manual-invoice`, request, { headers: headers, responseType: 'blob' as 'json' });
  }
}
