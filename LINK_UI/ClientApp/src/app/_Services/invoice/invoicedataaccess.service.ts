import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { InvoiceDataAccessEditModel, InvoiceDataAccessModel } from "src/app/_Models/invoice/invoice-data-access.model";

@Injectable({
    providedIn: 'root'
})

export class InoiceDataAccessService {

    url: string

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.url = baseUrl;
        if (this.url.charAt(this.url.length - 1) == '/')
            this.url = this.url.substring(0, this.url.length - 1);
    }

    async save(model: InvoiceDataAccessEditModel) {
       
        if (model.id > 0) {
            return await this.http.put<any>(`${this.url}/api/InvoiceDataAccess/update`, model)
                .toPromise();
        }

        else {
                return await this.http.post<any>
                    (`${this.url}/api/InvoiceDataAccess/save`, model).toPromise();
        }
    }

    async edit(id: number) {
        return await this.http.get<any>(`${this.url}/api/InvoiceDataAccess/edit/${id}`)
            .toPromise();
    }

    async getInvoiceDataAccessSummary(model: InvoiceDataAccessModel) {
        return await this.http.post<any>
            (`${this.url}/api/InvoiceDataAccess/getInvoiceDataAccessSummaryData`, model).toPromise();
    }

    async deleteInvoiceDataAccess(id: number) {
        return await this.http.delete<any>(`${this.url}/api/InvoiceDataAccess/delete/${id}`)
            .toPromise();
    }
}
