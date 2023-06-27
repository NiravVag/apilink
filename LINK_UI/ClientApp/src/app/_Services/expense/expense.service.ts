import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, first, catchError } from 'rxjs/operators';
import { ExpenseClaimModel, ExpenseClaimResponse, ExpenseClaimReceipt, ExpenseFoodClaimResponse, ExpenseFoodClaimRequest } from '../../_Models/expense/expenseclaim.model';
import { Observable } from 'rxjs';
import { ExpenseClaimListModel, ExpenseClaimUpdateStatus, PendingBookingExpenseRequest, PendingBookingExpenseResponse } from '../../_Models/expense/expenseclaimlist.model';
import { PendingExpenseModel } from 'src/app/_Models/expense/pendingexpense.model';

@Injectable({ providedIn: 'root' })
export class ExpenseService {
 
  url: string
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.url = baseUrl;

    if (this.url.charAt(this.url.length - 1) == '/')
      this.url = this.url.substring(0, this.url.length - 1);
  }

  getExpenseClaim(id?:number) {

    let currentService: string = id == null ? `${this.url}/api/expense/expense-claim` : `${this.url}/api/expense/expense-claim/${id}`;

    return this.http.get<ExpenseClaimResponse>(currentService)
      .pipe(map(response => {
        return response;
      }));
  }

  getExpenseSummary() {
    return this.http.get<any>(`${this.url}/api/expense/expense-summary`)
      .pipe(map(response => {
        return response;
      }));
  }

  getClaimList(model: ExpenseClaimListModel) {
    return this.http.post<any>(`${this.url}/api/expense/expense-list`, model)
      .pipe(map(response => {
        return response;
      }));
  }

  saveExpenseClaim(request: ExpenseClaimModel) {
    return this.http.post<any>(`${this.url}/api/expense/expense-claim/save`, request)
      .pipe(map(response => {
        return response;
      }));
  }

  saveOutSourceExpenseClaimList(request: ExpenseClaimModel) {
    return this.http.post<any>(`${this.url}/api/expense/expense-claim/save-outsource-qc-claim`, request)
      .pipe(map(response => {
        return response;
      }));
  }

  getCityList(term: string): Observable<any[]>{
    return this.http.get<any>(`${this.url}/api/expense/cities/${term}`)
      .pipe(map(response => {
        return response.items;
      }));
  }

  getCurrecyRate(targetId: number, currencyId: number, date : string) {

    let currentService: string = `${this.url}/api/expense/currency-rate/${targetId}/${currencyId}/${date}` ;

    return this.http.get<any>(currentService)
      .pipe(map(response => {
        return response;
      }));
  }

  getFoodAllowance(date: string, countryId: number) {
    let currentService: string = `${this.url}/api/expense/food-allowance/${date}/${countryId}`;

    return this.http.get<any>(currentService)
      .pipe(map(response => {
        return response;
      }));
  }

  getFile(id) {
    return this.http.get(`${this.url}/api/expense/file/${id}`, { responseType: 'blob' });
  }

  uploadFiles(fileList: Array<ExpenseClaimReceipt>) {
    const formData = new FormData();

    fileList.map(x => {
      formData.append(x.guidId, x.file);
    });

    return this.http.post(`${this.url}/api/expense/upload`, formData)
      .pipe(map(response => {
        return response;
      }));
  }

  setStatus(id: number, idStatus: number, expenseType: boolean) {
    return this.http.get<any>(`${this.url}/api/expense/status/${id}/${idStatus}/${expenseType}`)
      .pipe(map(response => {
        return response;
      }));
  }

  reject(id: number, comment: string) {
    return this.http.post<any>(`${this.url}/api/expense/reject/${id}`, { comment: comment })
      .pipe(map(response => {
        return response;
      }));
  }

  exportSummary(request: ExpenseClaimListModel) {

    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });

    return this.http.post<Blob>(`${this.url}/api/expense/exportSummary`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

  exportClaim(id: number) {
    return this.http.get(`${this.url}/api/expense/export-claim/${id}`, { responseType: 'blob' });
  }

  getClaimTypeList() {
    return this.http.get<any>(`${this.url}/api/expense/expense-claimtypelist`);
  }

  getBookingDetail(claimTypeId,expenseId,isEdit) {
    return this.http.get<any>(`${this.url}/api/expense/getbookingdetail/${claimTypeId}/${expenseId}/${isEdit}`);
  }

  exportVoucherKpiSummary(request: ExpenseClaimListModel) {

    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });

    return this.http.post<Blob>(`${this.url}/api/expense/vocherSummary`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

  exportExpenseKpiSummary(request: ExpenseClaimListModel) {

    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Accept': 'application/json'
    });

    return this.http.post<Blob>(`${this.url}/api/expense/ExpenseKpiSummary`, request, { headers: headers, responseType: 'blob' as 'json' });
  }

  getPendingExpenseList(model:PendingExpenseModel ) {

    return this.http.post<any>(`${this.url}/api/ScheduleJob/GetQcPendingExpenseData`, model)
      .pipe(map(response => {
        return response;
      }));  
  }

  saveQcPendingExpenseData(model:any ) {

    return this.http.post<any>(`${this.url}/api/ScheduleJob/SaveQcPendingExpenseData`, model)
      .pipe(map(response => {
        return response;
      }));  
  }

  checkPendingExpenseExist(model:any) {
    return this.http.post<any>(`${this.url}/api/expense/CheckPendingExpenseExist`, model)
      .pipe(map(response => {
        return response;
      }));
  } 
  async getExpenseFoodAmountByCountryAndDate(model: ExpenseFoodClaimRequest) : Promise<ExpenseFoodClaimResponse> {
    return this.http.post<ExpenseFoodClaimResponse>(`${this.url}/api/expense/expense-food-amount`, model).toPromise();
  }

  setStatusList(updateStatusList: Array<ExpenseClaimUpdateStatus>) {
    return this.http.post<any>(`${this.url}/api/expense/update-status`, updateStatusList)
      .pipe(map(response => {
        return response;
      }));
  }

  getPendingExpenseBookingIdList(pendingBookingExpenseRequest: Array<PendingBookingExpenseRequest>) {
    return this.http.post<PendingBookingExpenseResponse>(`${this.url}/api/expense/pending-expense-configure`, pendingBookingExpenseRequest)
      .pipe(map(response => {
        return response;
      }));
  }
}
