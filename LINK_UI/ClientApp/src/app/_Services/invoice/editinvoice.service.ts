import { Injectable, inject, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import {
    InvoiceBaseDetailResponse, InvoiceBilledAddressResponse, InvoiceContactsResponse,
    InvoiceTransactionDetailsResponse, UpdateInvoiceDetailRequest, UpdateInvoiceDetailsResponse,
    InvoiceBookingMoreInfoResponse, InvoiceBookingProductsResponse, DeleteInvoiceDetailResponse, InvoiceMoExistsResult, InvoiceNewBookingResponse, NewInvoiceBookingSearchRequest
} from '../../_Models/invoice/editinvoice.model';
import { InvoiceGenerateModel,InvoiceGenerateResponse } from 'src/app/_Models/invoice/invoicegenerate.model'
import { DataSourceResponse } from 'src/app/_Models/common/common.model';
import { map } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class EditInvoiceService {

    url: string
    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.url = baseUrl;

        if (this.url.charAt(this.url.length - 1) == '/')
            this.url = this.url.substring(0, this.url.length - 1);
    }

    getInvoiceBaseDetails(invoiceno,statusId) {
        return this.http.get<InvoiceBaseDetailResponse>(`${this.url}/api/Invoice/getinvoicebasedetails/${invoiceno}/${statusId}`);
    }

    getInvoiceTransactionDetails(invoiceno,statusId) {
        return this.http.get<InvoiceTransactionDetailsResponse>(`${this.url}/api/Invoice/getinvoicetransactiondetails/${invoiceno}/${statusId}`);
    }

    getInvoiceBillingAddress(invoiceToId, searchId) {
        return this.http.get<InvoiceBilledAddressResponse>(`${this.url}/api/Invoice/getbilledaddress/${invoiceToId}/${searchId}`);
    }

    getInvoiceContacts(invoiceToId, searchId) {
        return this.http.get<InvoiceContactsResponse>(`${this.url}/api/Invoice/getinvoicecontacts/${invoiceToId}/${searchId}`);
    }

    getInvoicePaymentStatus() {
        return this.http.get<DataSourceResponse>(`${this.url}/api/Invoice/getinvoicepaymentstatus`);
    }

    getInvoiceOffice() {
        return this.http.get<DataSourceResponse>(`${this.url}/api/Invoice/getinvoiceoffice`);
    }

    getBookingMoreInfo(bookingNo) {
        return this.http.get<InvoiceBookingMoreInfoResponse>(`${this.url}/api/Invoice/getinvoicebookingmoreinfo/${bookingNo}`);
    }

    getInvoiceBookingProducts(bookingNo) {
        return this.http.get<InvoiceBookingProductsResponse>(`${this.url}/api/Invoice/getinvoicebookingproducts/${bookingNo}`);
    }

    removeInvoiceBooking(invoiceId) {
        return this.http.delete<DeleteInvoiceDetailResponse>(`${this.url}/api/Invoice/${invoiceId}`);
    }

    async checkInvoiceNumberExist(invoiceNo): Promise<InvoiceMoExistsResult> {
        return await this.http.get<InvoiceMoExistsResult>(`${this.url}/api/Invoice/checkinvoicenumberexist/${invoiceNo}`).toPromise();
    }

    async save(model: UpdateInvoiceDetailRequest): Promise<UpdateInvoiceDetailsResponse> {
        return await this.http.post<UpdateInvoiceDetailsResponse>(`${this.url}/api/Invoice/updateinvoicedetails`, model).toPromise();
    }

    async  getBookingListForNewInvoice(model:NewInvoiceBookingSearchRequest) {
        return this.http.post<InvoiceNewBookingResponse>(`${this.url}/api/Invoice/getNewInvoiceBookingList`,model).toPromise();
    }

    /* async  generateInvoice(model:InvoiceGenerateModel) {
        return this.http.post<InvoiceNewBookingResponse>(`${this.url}/api/Invoice/generate`,model).toPromise();
    } */

    async generateInvoice(model: InvoiceGenerateModel): Promise<InvoiceGenerateResponse> {
        return await this.http.post<InvoiceGenerateResponse>(`${this.url}/api/Invoice/generate`, model).toPromise();
    }

    // getBookingInfo(bookingId) {
    //   return this.http.get<any>(`${this.url}/api/InspectionBooking/getBookingInfo/${bookingId}`);
    // }
}
