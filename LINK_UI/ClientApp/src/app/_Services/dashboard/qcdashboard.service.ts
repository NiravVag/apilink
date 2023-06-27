import { Injectable, inject, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import {QcDashboardSearchRequest} from '../../_Models/dashboard/qcdashboard.model';

@Injectable({
    providedIn: 'root'
})
export class QcDashBoardService {
    url: string
    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.url = baseUrl;

        if (this.url.charAt(this.url.length - 1) == '/')
            this.url = this.url.substring(0, this.url.length - 1);
    }
    getQcSchedule() {
        return this.http.get<any>(`${this.url}/api/QcDashboard/QcScheduleDetails`)
            .pipe(map(response => { return response; }));
    }
    getQcProductivityDetails(request:QcDashboardSearchRequest) {
        return this.http.post<any>(`${this.url}/api/QcDashboard/QcProductivityDetails`,request)
            .pipe(map(response => { return response; }));
    }
    getQcRejectionDetails(request:QcDashboardSearchRequest){
        return this.http.post<any>(`${this.url}/api/QcDashboard/QcRejectionDetails`,request)
        .pipe(map(response => { return response; }));
    }
    getQcDashboardCountDetails(request:QcDashboardSearchRequest){
        return this.http.post<any>(`${this.url}/api/QcDashboard/QcDashboardCountDetails`,request)
        .pipe(map(response => { return response; })); 
    }
     
}
