import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { InvoiceBankSaveRequest, InvoiceBankSaveResponse, InvoiceBankGetAllResponse, InvoiceBankGetResponse, InvoiceBankDeleteResponse, InvoiceBankSummary, InvoiceBankTaxRequest } from "src/app/_Models/invoice/invoicebank";

@Injectable({
    providedIn: 'root'
})

export class InvoiceBankService {

    url: string

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.url = baseUrl;
        if (this.url.charAt(this.url.length - 1) == '/')
            this.url = this.url.substring(0, this.url.length - 1);
    }

    async saveBankDetails(model: InvoiceBankSaveRequest): Promise<InvoiceBankSaveResponse> {
        return await this.http.post<InvoiceBankSaveResponse>
            (`${this.url}/api/InvoiceBank`, model).toPromise();
    }

    async getBankDetails(bankId: number) {
        return await this.http.get<InvoiceBankGetResponse>(`${this.url}/api/InvoiceBank/${bankId}`)
            .toPromise();
    }

    getAllBankDetails(model: InvoiceBankSummary) {

        return this.http.get<InvoiceBankGetAllResponse>(`${this.url}/api/InvoiceBank/${model.index}/${model.pageSize}`)
    }

    async updateBankDetails(model: InvoiceBankSaveRequest): Promise<InvoiceBankSaveResponse> {
        return await this.http.put<InvoiceBankSaveResponse>(`${this.url}/api/InvoiceBank/${model.id}`, model)
            .toPromise();
    }

    async deleteBankDetails(bankId: number) {
        return await this.http.delete<InvoiceBankDeleteResponse>(`${this.url}/api/InvoiceBank/${bankId}`)
            .toPromise();
    }

    async getBankTaxesByDate(bankId, model: InvoiceBankTaxRequest) {

        return this.http.post<InvoiceBankGetResponse>(`${this.url}/api/InvoiceBank/getTaxDetails/${bankId}`, model)
            .toPromise();
    }
}  
