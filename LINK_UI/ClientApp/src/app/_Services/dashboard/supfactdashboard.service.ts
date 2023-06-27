import { Injectable, inject, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { SupFactDashBoardModel } from 'src/app/_Models/dashboard/supfactdashboard.model';

@Injectable({
    providedIn: 'root'
})
export class SupFactDashboardService {
    url: string
    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.url = baseUrl;

        if (this.url.charAt(this.url.length - 1) == '/')
            this.url = this.url.substring(0, this.url.length - 1);
    }
    getBookingIds(request: SupFactDashBoardModel) {
        return this.http.post<any>(`${this.url}/api/SupFactDashboard/get-Booking-data`, request)
            .pipe(map(response => { return response; }));
    }
    getInspFactoryGeoCode(inspIdList: Array<number>) {
        return this.http.post<any>(`${this.url}/api/SupFactDashboard/get-factory-geo-code`, inspIdList)
            .pipe(map(response => { return response; }));
    }
    getInspCustomerData(inspIdList: Array<number>) {
        return this.http.post<any>(`${this.url}/api/SupFactDashboard/get-customer-data`, inspIdList)
            .pipe(map(response => { return response; }));
    }
    getInspDetails(inspIdList: Array<number>) {
        return this.http.post<any>(`${this.url}/api/SupFactDashboard/get-booking-details`, inspIdList)
            .pipe(map(response => { return response; }));
    }
}
