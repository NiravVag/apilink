import { Injectable, inject, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { CustomerDashboardFilterModel, DashboardMapFilterRequest } from 'src/app/_Models/dashboard/customerdashboardfilterrequest.model';
@Injectable({
    providedIn: 'root'
})
export class DashBoardService {
    url: string
    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.url = baseUrl;

        if (this.url.charAt(this.url.length - 1) == '/')
            this.url = this.url.substring(0, this.url.length - 1);
    }
    getBookingDetails(request) {
        return this.http.post<any>(`${this.url}/api/dashboard/getBookingDetails`, request)
            .pipe(map(response => { return response; }));
    }
    getCustomerBusinessOverview(request) {
        return this.http.post<any>(`${this.url}/api/dashboard/getcustomerbusinessoverview`, request)
            .pipe(map(response => { return response; }));
    }
    getAPIResult(request) {
        return this.http.post<any>(`${this.url}/api/dashboard/getAPIRAResult`, request)
            .pipe(map(response => { return response; }));
    }
    getCustomerResult(request) {
        return this.http.post<any>(`${this.url}/api/dashboard/getCustomerResult`, request)
            .pipe(map(response => { return response; }));
    }
    getInspectionRejectResult(request) {
        return this.http.post<any>(`${this.url}/api/dashboard/getinspectionreject`, request)
            .pipe(map(response => { return response; }));
    }
    getProductCategoryResult(request) {
        return this.http.post<any>(`${this.url}/api/dashboard/getproductcategoryData`, request)
            .pipe(map(response => { return response; }));
    }
    getSupplierPerformance(request) {
        return this.http.post<any>(`${this.url}/api/dashboard/getsupplierperformance`, request)
            .pipe(map(response => { return response; }));
    }
    getPendingQuotations(customerId) {
        return this.http.get<any>(`${this.url}/api/dashboard/getquotationtasks/${customerId}`);
    }
    getInspectedBookings(request) {
        return this.http.post<any>(`${this.url}/api/dashboard/getinspectedbookings`, request)
            .pipe(map(response => { return response; }));
    }
    getCustomerCsContact(customerId) {
        return this.http.get<any>(`${this.url}/api/dashboard/getcustomercscontact/${customerId}`);
    }
    

    getManDayDashBoard(mandaytype, customerId) {
        return this.http.get<any>(`${this.url}/api/dashboard/getmandaydata/${mandaytype}/${customerId}`);
    }
    getInspCountryGeoCode(request: DashboardMapFilterRequest) {
        return this.http.post<any>(`${this.url}/api/dashboard/getcountrygeocode`, request)
            .pipe(map(response => { return response; }));
    }
    getAllocatedInspCountryGeoCode() {
        return this.http.get<any>(`${this.url}/api/dashboard/getallocatedcountrygeocode`)
            .pipe(map(response => { return response; }));
    }

    
    getinspectedbookingsByFactory(request) {
        return this.http.post<any>(`${this.url}/api/dashboard/getinspectedbookingsByFactory`, request)
            .pipe(map(response => { return response; }));
    }

    getmandayByYear(request: CustomerDashboardFilterModel) {
      return this.http.post<any>(`${this.url}/api/dashboard/get-manday-count-by-year`, request)
            .pipe(map(response => {
                return response;
            }));
    }

    getCustomerDecisionCount(customerId) {
        return this.http.get<any>(`${this.url}/api/dashboard/getcustomerdecisioncount/${customerId}`);
    }
}
