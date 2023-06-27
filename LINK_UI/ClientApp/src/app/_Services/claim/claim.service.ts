import { Inject, Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { EditWorkLoadMatrixModel } from "src/app/_Models/workloadmatrix/workloadmatrix.model";
import { map } from "rxjs/operators";
import { BookingIdDataSourceRequest } from "src/app/_Models/common/common.model";
import { EditClaimModel } from "src/app/_Models/claim/edit-claim.model";
import { ClaimDataSourceResponse, ClaimDataSummaryResponse, ClaimSummaryModel, ClaimSummaryResponse } from "src/app/_Models/claim/claim-summary.model";
import { PendingClaimSummaryModel } from "src/app/_Models/claim/pending-claim-summary.model";
import { EditCreditNoteModel } from "src/app/_Models/claim/edit-credit-note.model";
import { CreditNoteSummaryModel } from "src/app/_Models/claim/credit-note-summary.model";

@Injectable({
    providedIn: 'root'
})

export class ClaimService {
    url: string;
    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.url = baseUrl;

        if (this.url.charAt(this.url.length - 1) == '/')
            this.url = this.url.substring(0, this.url.length - 1);
    }

    getBookingIdList(model: BookingIdDataSourceRequest, term?: string) {
        model.searchText = term ? term : "";
        if (term) {
            model.searchText = term;
            model.ids = [];
        }

        return this.http.post<any>(`${this.url}/api/Claim/get-booking-no`, model)
            .pipe(map(res => {
                return res.dataSourceList
            }));
    }

    getReportTitleList(bookingId: number) {
        return this.http.get<any>(`${this.url}/api/Claim/get-report-list/${bookingId}`)
            .pipe(map(res => {
                return res;
            }));
    }

    getClaimBookingData(request) {
        return this.http.post<any>(`${this.url}/api/Claim/get-booking-data`, request)
            .pipe(map(res => {
                return res;
            }));
    }

    getClaimFromList() {
        return this.http.get<any>(`${this.url}/api/Claim/get-claim-from-list`)
            .pipe(map(res => {
                return res;
            }));
    }

    getReceivedFromList() {
        return this.http.get<any>(`${this.url}/api/Claim/get-received-from-list`)
            .pipe(map(res => {
                return res;
            }));
    }
    getClaimSourceList() {
        return this.http.get<any>(`${this.url}/api/Claim/get-claim-source-list`)
            .pipe(map(res => {
                return res;
            }));
    }
    getDefectFamilyList() {
        return this.http.get<any>(`${this.url}/api/Claim/get-fb-defect-list`)
            .pipe(map(res => {
                return res;
            }));
    }
    getClaimDepartmentList() {
        return this.http.get<any>(`${this.url}/api/Claim/get-claim-department-list`)
            .pipe(map(res => {
                return res;
            }));
    }

    getCustomerRequestList() {
        return this.http.get<any>(`${this.url}/api/Claim/get-claim-customer-request-list`)
            .pipe(map(res => {
                return res;
            }));
    }

    getPriorityList() {
        return this.http.get<any>(`${this.url}/api/Claim/get-claim-priority-list`)
            .pipe(map(res => {
                return res;
            }));
    }

    getCustomerRequestRefundList() {
        return this.http.get<any>(`${this.url}/api/Claim/get-claim-refund-type-list`)
            .pipe(map(res => {
                return res;
            }));
    }

    getCurrencyList() {
        return this.http.get<any>(`${this.url}/api/Claim/get-currency-list`)
            .pipe(map(res => {
                return res;
            }));
    }

    getDefectDistributionList() {
        return this.http.get<any>(`${this.url}/api/Claim/get-claim-defect-distribution-list`)
            .pipe(map(res => {
                return res;
            }));
    }

    getClaimsByBookingId(bookingId: number) {
        return this.http.get<any>(`${this.url}/api/Claim/get-claims-list/${bookingId}`)
            .pipe(map(res => {
                return res;
            }));
    }

    getInvoiceByBookingId(bookingId: number) {
        return this.http.get<any>(`${this.url}/api/Claim/get-invoice-detail/${bookingId}`)
            .pipe(map(res => {
                return res;
            }));
    }

    getClaimResultList() {
        return this.http.get<any>(`${this.url}/api/Claim/get-claim-result-list`)
            .pipe(map(res => {
                return res;
            }));
    }

    getFinalResultList() {
        return this.http.get<any>(`${this.url}/api/Claim/get-final-result-list`)
            .pipe(map(res => {
                return res;
            }));
    }

    getFileTypeList() {
        return this.http.get<any>(`${this.url}/api/Claim/get-file-type-list`)
            .pipe(map(res => {
                return res;
            }));
    }

    saveClaim(model: EditClaimModel) {
        return this.http.post<any>(`${this.url}/api/Claim/claim/save`, model)
            .pipe(map(response => {
                return response;
            }));
    }

    getEditClaim(id) {

        if (id == null)
            id = "";

        return this.http.get<any>(`${this.url}/api/Claim/claim/edit/${id}`);
    }

    getClaimSummary() {
        return this.http.get<any>(`${this.url}/api/Claim/claim-summary`);
    }

    Getsupplierbycusid(cusid) {
        return this.http.get<any>(`${this.url}/api/Supplier/GetsupplierBycustomerid/${cusid}`).pipe(map(response => { return response; }));
    }

    getClaimDataSummary(model: ClaimSummaryModel) {
        return this.http.post<any>(`${this.url}/api/Claim/ClaimSummary`, model)
            .pipe(map(response => {
                return response;
            }));
    }

    exportSummary(request: ClaimSummaryModel) {
        const headers = new HttpHeaders({
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        });
        return this.http.post<Blob>(`${this.url}/api/Claim/ExportClaimSummary`, request, { headers: headers, responseType: 'blob' as 'json' });
    }

    cancelClaim(id: number) {
        return this.http.get<any>(`${this.url}/api/Claim/cancelClaim/${id}`)
            .pipe(map(response => {
                return response;
            }));
    }

    getPendingClaimSummary(model: PendingClaimSummaryModel) {
        return this.http.post<any>(`${this.url}/api/Claim/pending-claim-summary`, model)
            .pipe(map(response => {
                return response;
            }));
    }

    getClaimDetailByIds(claimIds: number[]) {
        return this.http.post<any>(`${this.url}/api/Claim/get-pending-claim`, claimIds)
            .pipe(map(response => {
                return response;
            }));
    }

    saveCreditNote(model: EditCreditNoteModel) {
        if (model && model.id > 0) {
            return this.http.put<any>(`${this.url}/api/Claim/update-credit-note`, model)
                .pipe(map(response => {
                    return response;
                }));
        } else {
            return this.http.post<any>(`${this.url}/api/Claim/save-credit-note`, model)
                .pipe(map(response => {
                    return response;
                }));
            
        }

    }
    getCreditNote(id: number) {
        return this.http.get<any>(`${this.url}/api/Claim/credit/${id}`)
            .pipe(map(response => {
                return response;
            }));
    }
    creditNoteSummary(model: CreditNoteSummaryModel) {
        return this.http.post<any>(`${this.url}/api/Claim/credit-note-summary`, model)
            .pipe(map(response => {
                return response;
            }));
    }

    getCreditTypeList() {
        return this.http.get<any>(`${this.url}/api/Claim/credit-type-list`)
            .pipe(map(response => {
                return response;
            }));
    }

    async checkCreditNumberExist(creditNo: string): Promise<boolean> {
        return this.http.get<any>(`${this.url}/api/Claim/check-creditno-exist/${creditNo}`).toPromise();
    }

    deleteCreditNote(id: number) {
        return this.http.delete<any>(`${this.url}/api/Claim/delete-credit-note/${id}`)
            .pipe(map(response => {
                return response;
            }));
    }
    exportCreditNoteSummary(model: CreditNoteSummaryModel){
        const headers = new HttpHeaders({
            'Content-Type': 'application/json',
            'Accept': 'application/json'
          });
          return this.http.post<Blob>(`${this.url}/api/Claim/export-credit-note`, model, { headers: headers, responseType: 'blob' as 'json' });
    }
}